namespace Defender.Common.Errors;

public enum ErrorCode
{
    Unknown,
    UnhandledError,


    #region Business rules


    #region Identity service

    BR_ACC,
    BR_ACC_InvalidPassword,
    BR_ACC_AdminCannotChangeAdminPassword,
    BR_ACC_UserCanUpdateOnlyOwnAccount,
    BR_ACC_UserIsBlocked,
    BR_ACC_SuperAdminCannotBeBlocked,
    BR_ACC_AdminCannotBlockAdmins,

    BR_ACC_AccessCodeWasExpired,
    BR_ACC_AccessCodeWasAlreadyUsed,
    BR_ACC_CodeWasNotVerified,
    BR_ACC_InvalidAccessCode,
    BR_ACC_CodeTypeMismatch,

    #endregion Identity service


    #region User management service

    BR_USM,
    BR_USM_EmailAddressInUse,
    BR_USM_PhoneNumberInUse,
    BR_USM_NicknameInUse,
    BR_USM_UserWithSuchLoginIsNotExist,

    #endregion User management service


    #region Walutomat helper service

    BR_WHS_NotSupportedCurrencyPair,

    #endregion Portal


    #region Wallet service


    BR_WLT,
    BR_WLT_CurrencyAccountAlreadyExist,
    BR_WLT_CurrencyAccountIsNotExist,
    BR_WLT_SenderCurrencyAccountIsNotExist,
    BR_WLT_RecipientCurrencyAccountIsNotExist,
    BR_WLT_NoAvailableWalletNumbers,
    BR_WLT_InvalidTransactionStatus,
    BR_WLT_WalletIsNotExist,
    BR_WLT_NotEnoughFunds,
    BR_WLT_SenderAndRecipientAreTheSame,
    BR_WLT_TransactionCanNotBeCanceled,


    #endregion


    #region Portal

    BR_PTL_UserActivityMustHaveUserId,

    #endregion Portal


    #region Risk games service

    BR_RGS_UnsupportedTransactionType,
    BR_RGS_UnsupportedGameType,
    BR_RGS_InvalidPaymentRequest,
    BR_RGS_InvalidTransactionAmount,
    BR_RGS_LotteryIsStillActive,
    BR_RGS_LotteryDrawIsNotActive,
    BR_RGS_CurrencyIsNotAllowed,
    BR_RGS_TryingToPurchaseInvalidTickets,
    BR_RGS_ThisBetIsNotAllowed,

    #endregion


    #endregion


    #region Validation


    VL_InvalidRequest,


    #region User management service


    VL_USM_EmptyUserId,
    VL_USM_EmptyLogin,
    VL_USM_EmptyNickname,
    VL_USM_EmptyEmail,
    VL_USM_EmptyAccessCode,

    VL_USM_InvalidPhoneNumber,
    VL_USM_InvalidEmail,

    VL_USM_MinNicknameLength,
    VL_USM_MaxNicknameLength,

    VL_USM_AtLeastOneFieldRequired,


    #endregion User management service


    #region Identity service


    VL_ACC_EmptyGoogleToken,
    VL_ACC_EmptyLogin,
    VL_ACC_EmptyEmail,
    VL_ACC_EmptyUserId,
    VL_ACC_EmptyNickname,
    VL_ACC_EmptyPassword,
    VL_ACC_EmptyAccessCode,

    VL_ACC_MinPasswordLength,
    VL_ACC_MaxPasswordLength,

    VL_ACC_MinNicknameLength,
    VL_ACC_MaxNicknameLength,

    VL_ACC_InvalidPhoneNumber,
    VL_ACC_InvalidEmail,


    #endregion Identity service


    #region Notification service


    VL_NTF_EmptyNotificationId,
    VL_NTF_EmptyRecipient,
    VL_NTF_EmptySubject,
    VL_NTF_EmptyBody,

    VL_NTF_MinSubjectLength,
    VL_NTF_MaxSubjectLength,

    VL_NTF_MaxBodyLength,

    VL_NTF_InvalidEmail,


    #endregion


    #region Wallet service


    VL_WLT_EmptyTransactionId,
    VL_WLT_EmptyWalletNumber,

    VL_WLT_InvalidWalletNumber,

    VL_WLT_TransferAmountMustBePositive,


    #endregion


    #region Secret management service


    VL_SCM_EmptySecretName,
    VL_SCM_EmptySecretValue,


    #endregion


    #region Wallet management service


    VL_WLT_EmptyUserId,


    #endregion


    #region Risk games service

    VL_RGS_InvalidDrawNumber,
    VL_RGS_InvalidTicketNumber,
    VL_RGS_InvalidAmount,
    VL_RGS_AmountOfTicketsMustBePositive,

    #endregion


    #endregion Validation


    #region External service exception


    ES_GoogleAPIIssue,
    ES_SendinBlueIssue,
    ES_WalutomatIssue,


    #endregion


    #region Common rules


    CM_InvalidUserJWT,
    CM_NotFound,
    CM_ForbiddenAccess,
    CM_DatabaseIssue,
    CM_MappingIssue,


    #endregion
}
