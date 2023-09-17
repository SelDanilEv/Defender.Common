﻿namespace Defender.Common.Errors
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

        #endregion Identity service


        #region User management service

        BR_USM_EmailAddressInUse,
        BR_USM_PhoneNumberInUse,
        BR_USM_NicknameInUse,
        BR_USM_UserWithSuchLoginIsNotExist,

        #endregion User management service

        #endregion

        #region Validation

        VL_InvalidRequest,

        #region User management service

        VL_USM_EmptyLogin,

        #endregion User management service


        #region Identity service

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

        #endregion Identity service

        #endregion Validation

        #region External service exception

        ES_GoogleAPIIssue,

        #endregion

        #region Common rules

        CM_ForbiddenAccess,

        #endregion
    }
}
