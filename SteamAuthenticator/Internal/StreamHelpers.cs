
namespace Steam_Authenticator.Internal
{
    internal static class StreamHelpers
    {
        public static void WriteBoolean(this Stream stream, bool value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static bool ReadBoolean(this Stream stream)
        {
            var data = new byte[1];
            stream.Read(data);
            return BitConverter.ToBoolean(data);
        }

        public static void WriteInt32(this Stream stream, int value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static int ReadInt32(this Stream stream)
        {
            var data = new byte[4];
            stream.Read(data);
            return BitConverter.ToInt32(data);
        }
    }
}
