namespace Defender.Common.Errors
{
    public enum ErrorCode
    {
        Unknown,
        UnhandledError,

        #region Business rules

        BR_ACC_InvalidPassword,
        BR_ACC_AdminCannotChangeAdminPassword,
        BR_ACC_UserCanUpdateOnlyOwnAccount,
        BR_ACC_UserIsBlocked,
        BR_ACC_SuperAdminCannotBeBlocked,
        BR_ACC_AdminCannotBlockAdmins,

        #endregion

        #region Validation

        VL_InvalidRequest,

        VL_ACC_EmptyGoogleToken,
        VL_ACC_EmptyLogin,
        VL_ACC_EmptyEmail,
        VL_ACC_EmptyNickname,
        VL_ACC_EmptyPassword,

        VL_ACC_MinPasswordLength,
        VL_ACC_MaxPasswordLength,

        VL_ACC_MinNicknameLength,
        VL_ACC_MaxNicknameLength,

        VL_ACC_InvalidPhoneNumber,
        VL_ACC_InvalidEmail,

        #endregion

        #region External service exception

        ES_GoogleAPIIssue,

        #endregion

        #region Common rules

        CM_ForbiddenAccess,

        #endregion
    }
}
