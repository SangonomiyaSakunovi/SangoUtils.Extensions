using Newtonsoft.Json;
using System;
using System.IO;

namespace SangoUtils.Utilitys
{
    public static class JsonNewton
    {
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T? FromJson<T>(string json) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static bool TryFromJson<T>(string json, out T? obj) where T : class
        {
            try
            {
                obj = JsonConvert.DeserializeObject<T>(json);
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
            return JsonConvert.DeserializeObject<T>(json);
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
                obj = JsonConvert.DeserializeObject<T>(json);
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
