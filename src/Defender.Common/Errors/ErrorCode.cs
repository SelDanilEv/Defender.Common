namespace Defender.Common.Errors
{
    public enum ErrorCode
    {
        Unknown,
        UnhandledError,

        #region Business rules

        #region Identity service

        BR_ACC_InvalidPassword,
        BR_ACC_AdminCannotChangeAdminPassword,
        BR_ACC_UserCanUpdateOnlyOwnAccount,
        BR_ACC_UserIsBlocked,
        BR_ACC_SuperAdminCannotBeBlocked,
        BR_ACC_AdminCannotBlockAdmins,

        BR_ACC_AccessCodeWasExpired,
        BR_ACC_AccessCodeWasAlreadyUsed,
        BR_ACC_AttemtsAreOver,
        BR_ACC_CodeWasNotVerified,

        #endregion Identity service

        #region User management service

        BR_USM_EmailAddressInUse,
        BR_USM_PhoneNumberInUse,
        BR_USM_NicknameInUse,
        BR_USM_UserWithSuchLoginIsNotExist,

        #endregion User management service

        #region Walutomat helper service

        BR_WHS_NotSupportedCurrencyPair,

        #endregion Portal

        #region Portal

        BR_PTL_UserActivityMustHaveUserId,

        #endregion Portal

        #endregion

        #region Validation

        VL_InvalidRequest,

        #region User management service

        VL_USM_EmptyUserId,
        VL_USM_EmptyLogin,

        #endregion User management service

        #region Identity service

        VL_ACC_EmptyGoogleToken,
        VL_ACC_EmptyLogin,
        VL_ACC_EmptyEmail,
        VL_ACC_EmptyUserId,
        VL_ACC_EmptyNickname,
        VL_ACC_EmptyPassword,

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
        VL_NTF_EmptyValidationLink,

        VL_NTF_MinSubjectLength,
        VL_NTF_MaxSubjectLength,

        VL_NTF_MaxBodyLength,

        VL_NTF_InvalidEmail,

        #endregion

        #region Secret management service

        VL_SCM_EmptySecretName,
        VL_SCM_EmptySecretValue,

        #endregion

        #endregion Validation

        #region External service exception

        ES_GoogleAPIIssue,
        ES_SendinBlueIssue,

        #endregion

        #region Common rules

        CM_ForbiddenAccess,
        CM_DatabaseIssue,
        CM_MappingIssue,

        #endregion
    }
}
