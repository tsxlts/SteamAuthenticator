
using ProtoBuf;
using System.Text;

namespace Steam_Authenticator.Internal
{
    internal class Manifest
    {
        private readonly SemaphoreSlim locker = new SemaphoreSlim(1, 1);

        public static Manifest FromFile(string file) => new Manifest(file);

        private Manifest(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            if (!fileInfo.Exists)
            {
                using (fileInfo.Create()) { };
            }

            FileName = file;
            Load();
        }

        public IEnumerable<string> GetEntries(string path)
        {
            var entries = Entries.Where(c => c.Path.Equals(path, StringComparison.OrdinalIgnoreCase));
            return entries.Select(c => c.Name);
        }

        public T GetEntry<T>(string path, string name, string password) where T : IStreamSerializer, new()
        {
            var manifestEntry = Entries.FirstOrDefault(c => c.Path.Equals(path, StringComparison.OrdinalIgnoreCase) && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (manifestEntry == null)
            {
                return default;
            }

            byte[] decrypted = manifestEntry.Data;
            if (Encrypted)
            {
                decrypted = FileEncryptor.DecryptData(password, Salt, IV, decrypted);
            }

            using (var stream = new MemoryStream(decrypted))
            {
                T entry = new T();
                entry.Deserialize(stream);
                return entry;
            }
        }

        public bool SaveEntry<T>(string path, string name, string password, T entry) where T : IStreamSerializer
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, "path");
            ArgumentException.ThrowIfNullOrWhiteSpace(name, "name");

            using (var stream = entry.Serialize())
            {
                byte[] encrypted = new byte[stream.Length];
                stream.Read(encrypted);

                if (Encrypted)
                {
                    encrypted = FileEncryptor.EncryptData(password, Salt, IV, encrypted);
                }
                if (encrypted == null)
                {
                    return false;
                }

                ManifestEntry manifestEntry = new ManifestEntry()
                {
                    Path = path,
                    Name = name,
                    Data = encrypted
                };

                int index = Entries.FindIndex(c => c.Path.Equals(path, StringComparison.OrdinalIgnoreCase) && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                Entries.RemoveAll(c => c.Path.Equals(path, StringComparison.OrdinalIgnoreCase) && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (index >= 0)
                {
                    Entries.Insert(index, manifestEntry);
                }
                else
                {
                    Entries.Add(manifestEntry);
                }

                return Save();
            }
        }

        public bool RemoveEntry<T>(string path, string name, string password, out T entry) where T : IStreamSerializer, new()
        {
            entry = GetEntry<T>(path, name, password);
            if (entry == null)
            {
                return true;
            }

            ManifestEntry manifestEntry = Entries.First(c => c.Path.Equals(path, StringComparison.OrdinalIgnoreCase) && c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            Entries.Remove(manifestEntry);
            return Save();
        }

        public bool CheckPassword(string password)
        {
            if (!Encrypted)
            {
                return true;
            }

            try
            {
                byte[] decrypted = FileEncryptor.DecryptData(password, Salt, IV, Sign);
                var sign = Encoding.UTF8.GetString(decrypted);
                return sign == password;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            if (IV.Length == 0 || Salt.Length == 0)
            {
                IV = FileEncryptor.GetInitializationVector();
                Salt = FileEncryptor.GetRandomSalt();
            }

            List<ManifestEntry> entries = new List<ManifestEntry>();

            ManifestEntry manifestEntry;
            foreach (var entry in Entries)
            {
                manifestEntry = new ManifestEntry
                {
                    Path = entry.Path,
                    Name = entry.Name,
                    Data = entry.Data
                };

                if (Encrypted)
                {
                    manifestEntry.Data = FileEncryptor.DecryptData(oldPassword, Salt, IV, manifestEntry.Data);
                }
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    manifestEntry.Data = FileEncryptor.EncryptData(newPassword, Salt, IV, manifestEntry.Data);
                }

                entries.Add(manifestEntry);
            }

            Encrypted = !string.IsNullOrWhiteSpace(newPassword);
            Entries = entries;

            Sign = new byte[0];
            if (Encrypted)
            {
                var signBuffer = Encoding.UTF8.GetBytes(newPassword);
                signBuffer = FileEncryptor.EncryptData(newPassword, Salt, IV, signBuffer);
                Sign = signBuffer;
            }

            return Save();
        }

        private bool Save()
        {
            if (!locker.Wait(3000))
            {
                return false;
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.WriteBoolean(Encrypted);

                    stream.WriteInt32(IV.Length);
                    stream.WriteInt32(Salt.Length);

                    stream.Write(IV);
                    stream.Write(Salt);

                    stream.WriteInt32(Sign.Length);
                    stream.Write(Sign);

                    stream.Write(new byte[] { 0x0A });

                    using (var headerStream = new MemoryStream())
                    {
                        Serializer.Serialize(headerStream, Header);
                        var headerBuffer = headerStream.ToArray();
                        stream.WriteInt32(headerBuffer.Length);
                        stream.Write(headerBuffer);
                    }

                    stream.Write(new byte[] { 0x0A });

                    byte[] pathBuffer;
                    byte[] nameBuffer;
                    byte[] dataBuffer;
                    foreach (ManifestEntry entry in Entries)
                    {
                        pathBuffer = Encoding.UTF8.GetBytes(entry.Path);
                        nameBuffer = Encoding.UTF8.GetBytes(entry.Name ?? "");
                        dataBuffer = entry.Data;

                        stream.WriteInt32(pathBuffer.Length);
                        stream.WriteInt32(nameBuffer.Length);
                        stream.WriteInt32(dataBuffer.Length);

                        stream.Write(new byte[] { 0x0A });
                        stream.Write(pathBuffer);
                        stream.Write(new byte[] { 0x0A });
                        stream.Write(nameBuffer);
                        stream.Write(new byte[] { 0x0A });
                        stream.Write(dataBuffer);
                        stream.Write(new byte[] { 0x0A });
                    }

                    using (var fileStream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        fileStream.SetLength(0);

                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }
                }

                return true;
            }
            finally
            {
                locker.Release();
            }
        }

        private void Load()
        {
            using (var stream = File.OpenRead(FileName))
            {
                Encrypted = stream.ReadBoolean();

                byte[] ivBuffer = new byte[stream.ReadInt32()];
                byte[] saltBuffer = new byte[stream.ReadInt32()];

                stream.Read(ivBuffer);
                stream.Read(saltBuffer);

                IV = ivBuffer;
                Salt = saltBuffer;

                byte[] signBuffer = new byte[stream.ReadInt32()];
                stream.Read(signBuffer);
                Sign = signBuffer;

                stream.ReadByte();

                var headerBuffer = new byte[stream.ReadInt32()];
                stream.Read(headerBuffer);
                using (var headerStream = new MemoryStream(headerBuffer))
                {
                    Header = new ManifestHeader();
                    Serializer.Deserialize(headerStream, Header);
                }

                stream.ReadByte();

                byte[] pathBuffer;
                byte[] nameBuffer;
                byte[] dataBuffer;
                while (stream.Position != stream.Length)
                {
                    pathBuffer = new byte[stream.ReadInt32()];
                    nameBuffer = new byte[stream.ReadInt32()];
                    dataBuffer = new byte[stream.ReadInt32()];

                    stream.ReadByte();
                    stream.Read(pathBuffer);
                    stream.ReadByte();
                    stream.Read(nameBuffer);
                    stream.ReadByte();
                    stream.Read(dataBuffer);
                    stream.ReadByte();

                    Entries.Add(new ManifestEntry
                    {
                        Path = Encoding.UTF8.GetString(pathBuffer),
                        Name = Encoding.UTF8.GetString(nameBuffer),
                        Data = dataBuffer
                    });
                }
            }
        }

        public bool Encrypted { get; set; }

        public byte[] IV { get; set; } = new byte[0];

        public byte[] Salt { get; set; } = new byte[0];

        public byte[] Sign { get; set; } = new byte[0];

        public string FileName { get; set; }

        public ManifestHeader Header { get; set; } = new ManifestHeader
        {
            Version = 1
        };

        public List<ManifestEntry> Entries { get; private set; } = new List<ManifestEntry>();

        public class ManifestEntry
        {
            public string Path { get; set; }

            public string Name { get; set; }

            public byte[] Data { get; set; }
        }

        [ProtoContract()]
        public class ManifestHeader : IExtensible
        {
            private IExtension __pbn__extensionData;
            IExtension IExtensible.GetExtensionObject(bool createIfMissing) => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [ProtoMember(1)]
            public uint Version { get; set; }
        }
    }
}
