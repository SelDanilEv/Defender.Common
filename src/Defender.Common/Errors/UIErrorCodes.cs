namespace Defender.Common.Errors;

public enum UIErrorCodes
{
    UnhandledError,
    ForbiddenAccess,
    AuthorizationFailed,

    SessionExpired,

    EmptyLogin,
    EmptyEmail,
    EmptyNickname,
    EmptyPassword,
    EmptyWalletNumber,

    InvalidEmail,
    InvalidPhoneNumber,
    InvalidLoginOrPassword,
    InvalidWalletNumber,

    PasswordIsTooShort,
    PasswordIsTooLong,

    EmailAddressInUse,
    PhoneNumberInUse,
    NicknameInUse,

    NicknameIsTooShort,
    NicknameIsTooLong,

    AccessCodeWasExpired,
    AccessCodeWasAlreadyUsed,
    InvalidAccessCode,

    UserBlocked,

    WalletIsNotExist,
    SenderAndRecipientAreTheSame,
    RecipientCurrencyAccountIsNotExist,
    NotEnoughFunds,
    CurrencyAccountNotFound,
}
