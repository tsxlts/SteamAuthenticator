using Steam_Authenticator.Internal;

namespace Steam_Authenticator.Model
{
    internal class ManifestPasswordInput
    {
        public string GetPassword(Manifest manifest)
        {
            if (!manifest.Encrypted)
            {
                return null;
            }

            return Appsetting.Instance.AppSetting.Password;
        }
    }
}
