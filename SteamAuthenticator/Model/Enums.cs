

namespace Steam_Authenticator.Model
{
    internal enum MoveAuthenticatorResult
    {
        Begin,
        WaitAddPhone,
        WaitEmailConfirm,
        WaitFinalizationAddPhone,
        AddPhoneFailure,
        WaitFinalization
    }
}
