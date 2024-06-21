using System;
using System.Text.RegularExpressions;

namespace SangoUtils.Behaviours.FileOPs
{
    public static class Base64OP
    {
        public static string EncodeNoHeader(byte[] bytes)
        {
            string base64str = Convert.ToBase64String(bytes);
            return Regex.Replace(base64str, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);
        }
    }
}
