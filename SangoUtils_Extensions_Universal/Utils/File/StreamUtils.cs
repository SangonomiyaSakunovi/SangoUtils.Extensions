using System.IO;
using System.Text;

namespace SangoUtils_Extensions_Universal.Utils
{
    public static class StreamUtils
    {
        public static byte[] GetBytes(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return bytes;
        }

        public static byte[] GetBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(this Stream stream)
        {
            return Encoding.UTF8.GetString(stream.GetBytes());
        }
    }
}
