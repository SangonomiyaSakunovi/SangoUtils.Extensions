using SangoUtils_Common.Utils;
using System;
using System.Text.Json.Serialization;

namespace SangoUtils_Common
{
#pragma warning disable CS8618

    [Serializable]
    public class LoginReqData
    {
        [JsonConverter(typeof(NullableEnumConverter<LoginMode>))]
        public LoginMode LoginMode { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string UID { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string Password { get; set; }
    }

    public class LoginReqInfo
    {
        public LoginReqInfo(LoginMode loginMode, string uID, string password)
        {
            LoginMode = loginMode;
            UID = uID;
            Password = password;
        }

        public LoginMode LoginMode { get; set; }
        public string UID { get; set; }
        public string Password { get; set; }
    }

    [Serializable]
    public class LoginRspData
    {
        [JsonConverter(typeof(NullableEnumConverter<LoginResCode>))]
        public LoginResCode LoginResCode { get; set; }
    }

    public class LoginRspInfo
    {
        public LoginRspInfo(LoginResCode loginResCode)
        {
            LoginResCode = loginResCode;
        }

        public LoginResCode LoginResCode { get; private set; }
    }

    public class RegistReqData
    {

    }

    public class RegistReqInfo
    {

    }

    public class RegistRspData
    {

    }

    public class RegistRspInfo
    {

    }

    public enum LoginMode
    {
        None,
        Guest,
        UIDAndPassword
    }

    public enum LoginResCode
    {
        None,
        LoginSuccess,
        LoginFailed_AccountHasOnline,
        LoginFailed_UIDAndPasswordNotMatch
    }
#pragma warning restore CS8618
}
