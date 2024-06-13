namespace Defender.Common.Errors;

public enum UIErrorCodes
{
    Error_UnhandledError,
    Error_ForbiddenAccess,

    Error_SessionExpired,

    Error_EmptyLogin,
    Error_EmptyEmail,
    Error_EmptyNickname,
    Error_EmptyPassword,
    Error_EmptyWalletNumber,

    Error_InvalidEmail,
    Error_InvalidPhoneNumber,
    Error_InvalidLoginOrPassword,
    Error_InvalidWalletNumber,
    Error_WalletAccountNotFound,

    Error_PasswordIsTooShort,
    Error_PasswordIsTooLong,

    Error_EmailAddressInUse,
    Error_PhoneNumberInUse,
    Error_NicknameInUse,

    Error_NicknameIsTooShort,
    Error_NicknameIsTooLong,

    Error_AccessCodeWasExpired,
    Error_AccessCodeWasAlreadyUsed,
    Error_InvalidAccessCode,

    Error_WalletIsNotExist,

    Error_UserBlocked,
}
