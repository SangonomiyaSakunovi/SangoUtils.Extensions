using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SangoUtils_Extensions_UnityEngine.Utils
{
    public static class TextureUtils
    {
        private const string _texture2DType = "*.PNG|*.JPG";

        public static List<Texture>? FromTextureFolder(string folderPath, int width, int length)
        {
            if (!Directory.Exists(folderPath))
            {
                return null;
            }
            List<string> texturePaths = new List<string>();
            string[] textureTypes = _texture2DType.Split('|');
            for (int i = 0; i < textureTypes.Length; i++)
            {
                string[] textureDirs = Directory.GetFiles(folderPath, textureTypes[i]);
                for (int j = 0; j < textureDirs.Length; j++)
                {
                    texturePaths.Add(textureDirs[j]);
                }
            }
            List<Texture> textureResults = new List<Texture>();
            for (int k = 0; k < texturePaths.Count; k++)
            {
                Texture2D texture = new Texture2D(width, length);
                byte[] textureByte = FileStreamUtils_GetBytes(texturePaths[k]);
                texture.LoadImage(textureByte);
                textureResults.Add(texture);
            }
            return textureResults;
        }

        public static Texture2D ToTexture2D(this byte[] bytes)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            return texture;
        }

        public static Texture2D ToTexture2D(this string base64)
        {
            return Convert.FromBase64String(base64).ToTexture2D();
        }

        public static string ToBase64(this Texture2D texture)
        {
            return Convert.ToBase64String(texture.EncodeToPNG());
        }

        private static byte[] FileStreamUtils_GetBytes(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            return bytes;
        }
    }
}
