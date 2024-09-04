
namespace Steam_Authenticator.Internal
{
    internal class FileEncryptException : Exception
    {
        public FileEncryptException(string message) : base(message)
        {
        }

        public FileEncryptException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
