using System;
using System.Text.Json.Serialization;
using SangoUtils_Common.Utils;

namespace SangoUtils_Common
{
#pragma warning disable CS8618

    [Serializable]
    public class UserData
    {
        [JsonConverter(typeof(NullableStringConverter))]
        public string UID { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string Account { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string Password { get; set; }
    }

    public class UserInfo
    {
        public UserInfo(string uID, string account, string password)
        {
            UID = uID;
            Account = account;
            Password = password;
        }

        public string UID { get; private set; }
        public string Account { get; private set; }
        public string Password { get; private set; }
    }
#pragma warning restore CS8618
}
