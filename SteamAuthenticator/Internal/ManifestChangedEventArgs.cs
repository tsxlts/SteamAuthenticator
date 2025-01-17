
namespace Steam_Authenticator.Internal
{
    internal class ManifestChangedEventArgs : EventArgs
    {
        public ManifestChangedEventArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
