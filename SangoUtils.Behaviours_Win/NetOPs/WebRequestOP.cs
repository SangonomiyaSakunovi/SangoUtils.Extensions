using System.Net;
using System.Text;
using System.Text.Json;

namespace SangoUtils.Behaviours_Win.NetOPs
{
    public enum HttpMessageContentType
    {
        Json,
        FormUrlEncoded
    }

    public static class WebRequestOP
    {
        private static string Serilize(Dictionary<string, string> keyValuePairs, HttpMessageContentType reqType = HttpMessageContentType.Json)
        {
            return reqType switch
            {
                HttpMessageContentType.Json => JsonSerializer.Serialize(keyValuePairs),
                HttpMessageContentType.FormUrlEncoded => GetFormUrlEncode(keyValuePairs),
                _ => JsonSerializer.Serialize(keyValuePairs)
            };
        }

        private static string GetFormUrlEncode(Dictionary<string, string> keyValuePairs)
        {
            if (keyValuePairs == null || keyValuePairs.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                bool isFirst = true;
                foreach (var item in keyValuePairs)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        stringBuilder.Append('&');
                    }
                    stringBuilder.Append(Uri.EscapeDataString(item.Key));
                    stringBuilder.Append('=');
                    stringBuilder.Append(Uri.EscapeDataString(item.Value));
                }
                return stringBuilder.ToString();
            }
        }

        public static string Post(string postUrl, Dictionary<string, string> keyValuePairs, Encoding dataEncode, 
            HttpMessageContentType reqType = HttpMessageContentType.Json, HttpMessageContentType rspType = HttpMessageContentType.Json)
        {
            return Post(postUrl, Serilize(keyValuePairs, reqType), dataEncode, reqType, rspType);
        }

        public static string Post(string postUrl, string paramData, Encoding dataEncode, 
            HttpMessageContentType reqType = HttpMessageContentType.Json, HttpMessageContentType rspType = HttpMessageContentType.Json)
        {
            try
            {
                byte[] bytesData = dataEncode.GetBytes(paramData);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";

                string reqContentType = reqType switch
                {
                    HttpMessageContentType.Json => "application/json",
                    HttpMessageContentType.FormUrlEncoded => "application/x-www-form-urlencoded",
                    _ => "application/json"
                };

                webReq.ContentType = reqContentType;
                webReq.ContentLength = bytesData.Length;

                string rspContentType = rspType switch
                {
                    HttpMessageContentType.Json => "application/json",
                    HttpMessageContentType.FormUrlEncoded => "application/x-www-form-urlencoded",
                    _ => "application/json"
                };

                webReq.Accept = rspContentType;

                using Stream reqStream = webReq.GetRequestStream();
                reqStream.Write(bytesData, 0, bytesData.Length);

                using HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                using StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                return sr.ReadToEnd().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Post(string postUrl, byte[] bytesData)
        {
            try
            {
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";

                using Stream reqStream = webReq.GetRequestStream();
                reqStream.Write(bytesData, 0, bytesData.Length);

                using HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                using StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                return sr.ReadToEnd().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
