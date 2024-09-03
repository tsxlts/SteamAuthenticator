
using System.Security.Cryptography;

namespace Steam_Authenticator.Internal
{
    internal class FileEncryptor
    {
        private const int PBKDF2_ITERATIONS = 50000;
        private const int SALT_LENGTH = 8;
        private const int KEY_SIZE_BYTES = 32;
        private const int IV_LENGTH = 16;

        /// <summary>
        /// 获取8位随机加密盐
        /// </summary>
        /// <returns></returns>
        public static byte[] GetRandomSalt()
        {
            byte[] salt = new byte[SALT_LENGTH];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// 返回base64编码的16字节加密随机初始化矢量（IV）
        /// </summary>
        /// <returns></returns>
        public static byte[] GetInitializationVector()
        {
            byte[] iv = new byte[IV_LENGTH];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        /// <summary>
        /// 生成加密密钥，该密钥使用密码、随机加密盐和指定的PBKDF2轮数派生
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static byte[] GetEncryptionKey(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is empty");
            }
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, PBKDF2_ITERATIONS, HashAlgorithmName.SHA1))
            {
                return pbkdf2.GetBytes(KEY_SIZE_BYTES);
            }
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iv">Initialization Vector</param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] DecryptData(string password, byte[] salt, byte[] iv, byte[] buffer)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is empty");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("Buffer data is empty");
            }

            byte[] key = GetEncryptionKey(password, salt);
            using (var aes256 = Aes.Create())
            {
                aes256.Padding = PaddingMode.PKCS7;
                aes256.Mode = CipherMode.CBC;
                aes256.IV = iv;
                aes256.Key = key;
                using (ICryptoTransform decryptor = aes256.CreateDecryptor(aes256.Key, aes256.IV))
                {
                    byte[] cipherBytes = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                    return cipherBytes;
                }
            }
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iv"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] EncryptData(string password, byte[] salt, byte[] iv, byte[] buffer)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is empty");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("Buffer data is empty");
            }
            byte[] key = GetEncryptionKey(password, salt);

            using (var aes256 = Aes.Create())
            {
                aes256.Key = key;
                aes256.IV = iv;
                aes256.Mode = CipherMode.CBC;
                aes256.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes256.CreateEncryptor(aes256.Key, aes256.IV))
                {
                    byte[] cipherBytes = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
                    return cipherBytes;
                }
            }
        }
    }
}
