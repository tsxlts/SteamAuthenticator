
namespace Steam_Authenticator.Internal
{
    internal class FileDecryptException : Exception
    {
        public FileDecryptException(string message) : base(message)
        {
        }

        public FileDecryptException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
