using SangoUtils_Common.Utils;
using System;
using System.Text.Json.Serialization;

namespace SangoUtils_Common
{
#pragma warning disable CS8618

    [Serializable]
    public class UserInfo
    {
        public UserInfo(string uID, string account, string password)
        {
            UID = uID;
            Account = account;
            Password = password;
        }

        [JsonConverter(typeof(NullableStringConverter))]
        public string UID { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string Account { get; set; }
        [JsonConverter(typeof(NullableStringConverter))]
        public string Password { get; set; }
    }
#pragma warning restore CS8618
}
