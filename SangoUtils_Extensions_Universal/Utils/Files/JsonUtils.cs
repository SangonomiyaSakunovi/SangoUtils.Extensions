using System;
using System.IO;
using System.Text.Json;

namespace SangoUtils.Extensions.Utils
{
    public static class JsonUtils
    {
        public static string ToJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T? FromJson<T>(string json) where T : class
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static bool TryFromJson<T>(string json, out T? obj) where T : class
        {
            try
            {
                obj = JsonSerializer.Deserialize<T>(json);
                return true;
            }
            catch (Exception)
            {
                obj = default;
                return false;
            }
        }

        public static T? FromJsonFile<T>(string jsonPath) where T : class
        {
            string json = string.Empty;
            using (StreamReader sr = File.OpenText(jsonPath))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }
            return JsonSerializer.Deserialize<T>(json);
        }

        public static bool TryFromJsonFile<T>(string jsonPath, out T? obj) where T : class
        {
            try
            {
                string json = string.Empty;
                using (StreamReader sr = File.OpenText(jsonPath))
                {
                    json = sr.ReadToEnd();
                    sr.Close();
                }
                obj = JsonSerializer.Deserialize<T>(json);
                return true;
            }
            catch (Exception)
            {
                obj = default;
                return false;
            }
        }
    }
}
