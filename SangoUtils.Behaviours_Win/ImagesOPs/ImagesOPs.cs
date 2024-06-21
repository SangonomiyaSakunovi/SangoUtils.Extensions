using System.Drawing;

namespace SangoUtils.Behaviours_Win.ImagesOPs
{
    public static class YUVHelper
    {
        private static double[,] YUV2RGB_CONVERT_MATRIX = new double[3, 3] { { 1, 0, 1.4022 }, { 1, -0.3456, -0.7145 }, { 1, 1.771, 0 } };

        /// <summary>
        /// YUV420图片字节数据保存为.bmp图片
        /// </summary>
        /// <param name="rgbFrame">YUV420图片数组</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="bmpFilePath">文件存储路径(*.bmp)</param>
        public static void YUV420SaveAsBMPFile(byte[] yuv420Frames, int width, int height, string bmpFilePath)
        {
            try
            {
                byte[] rgbFrame = YUV420ToRGB(yuv420Frames, width, height);

                int yu = width * 3 % 4;
                int bytePerLine = 0;
                yu = yu != 0 ? 4 - yu : yu;
                bytePerLine = width * 3 + yu;

                using FileStream fs = File.Open(bmpFilePath, FileMode.Create);
                using BinaryWriter bw = new BinaryWriter(fs);
                #region 文件头14字节

                bw.Write('B');
                bw.Write('M');
                bw.Write(bytePerLine * height + 54); //文件总长度
                bw.Write(0);
                bw.Write(54); //图像数据地址 

                #endregion

                #region 位图信息头40字节

                bw.Write(40); //信息头长度
                bw.Write(width); //位图宽度（像素）
                bw.Write(height); //位图高度（像素);
                bw.Write((ushort)1); // 总是1
                bw.Write((ushort)24); //色深 2的24次方，即24位彩色
                bw.Write(0); //压缩方式 0 不压缩
                bw.Write(bytePerLine * height); //图像数据大小（字节）
                bw.Write(0); //水平分辨率
                bw.Write(0); //垂直分辨率
                bw.Write(0); //图像使用的颜色数，0全部使用
                bw.Write(0); //重要的颜色数，0全部都重要

                #endregion

                byte[] data = new byte[bytePerLine * height];
                int gIndex = width * height;
                int bIndex = gIndex * 2;

                for (int y = height - 1, j = 0; y >= 0; y--, j++)
                {
                    for (int x = 0, i = 0; x < width; x++)
                    {
                        data[y * bytePerLine + i++] = rgbFrame[bIndex + j * width + x];    // B
                        data[y * bytePerLine + i++] = rgbFrame[gIndex + j * width + x];    // G
                        data[y * bytePerLine + i++] = rgbFrame[j * width + x];  // R
                    }
                }

                bw.Write(data, 0, data.Length);
                bw.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// YUV420图片字节流转换成System.Drawing.Bitmap对象
        /// </summary>
        /// <param name="yuv420Frame">YUV420图片数组</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns></returns>
        public static Bitmap YUV420FrameToImage(byte[] yuv420Frame, int width, int height)
        {
            try
            {
                byte[] rgbFrame = YUV420ToRGB(yuv420Frame, width, height);
                Bitmap rev = null;

                int yu = width * 3 % 4;
                int bytePerLine = 0;
                yu = yu != 0 ? 4 - yu : yu;
                bytePerLine = width * 3 + yu;

                using MemoryStream ms = new MemoryStream();
                using BinaryWriter bw = new BinaryWriter(ms);
                #region 文件头14字节

                bw.Write('B');
                bw.Write('M');
                Console.WriteLine(bytePerLine * height + 54);
                bw.Write(bytePerLine * height + 54); //文件总长度
                bw.Write(0);
                bw.Write(54); //图像数据地址 

                #endregion

                #region 位图信息头40字节

                bw.Write(40); //信息头长度
                bw.Write(width); //位图宽度（像素）
                bw.Write(height); //位图高度（像素);
                bw.Write((ushort)1); // 总是1
                bw.Write((ushort)24); //色深 2的24次方，即24位彩色
                bw.Write(0); //压缩方式 0 不压缩
                bw.Write(bytePerLine * height); //图像数据大小（字节）
                bw.Write(0); //水平分辨率
                bw.Write(0); //垂直分辨率
                bw.Write(0); //图像使用的颜色数，0全部使用
                bw.Write(0); //重要的颜色数，0全部都重要

                #endregion

                byte[] data = new byte[bytePerLine * height];
                int gIndex = width * height;
                int bIndex = gIndex * 2;

                for (int y = height - 1, j = 0; y >= 0; y--, j++)
                {
                    for (int x = 0, i = 0; x < width; x++)
                    {
                        data[y * bytePerLine + i++] = rgbFrame[bIndex + j * width + x];    // B
                        data[y * bytePerLine + i++] = rgbFrame[gIndex + j * width + x];    // G
                        data[y * bytePerLine + i++] = rgbFrame[j * width + x];  // R
                    }
                }

                bw.Write(data, 0, data.Length);
                bw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                rev = new Bitmap(new Bitmap(ms, false));
                return rev;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// YUV420图片字节流转换成 RGB图片字节流（不含文件头及位图信息）
        /// </summary>
        /// <param name="yuv420Frames">YUV420图片数组</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns>RGB图片字节数组</returns>
        private static byte[] YUV420ToRGB(byte[] yuv420Frames, int width, int height)
        {
            byte[] rgbFrame = new byte[width * height * 3];

            int uIndex = width * height;
            int vIndex = uIndex + ((width * height) >> 2);
            int gIndex = width * height;
            int bIndex = gIndex * 2;

            int temp = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // R
                    temp = (int)(yuv420Frames[y * width + x] + (yuv420Frames[vIndex + (y / 2) * (width / 2) + x / 2] - 128) * YUV2RGB_CONVERT_MATRIX[0, 2]);
                    rgbFrame[y * width + x] = (byte)(temp < 0 ? 0 : (temp > 255 ? 255 : temp));

                    // G
                    temp = (int)(yuv420Frames[y * width + x] + (yuv420Frames[uIndex + (y / 2) * (width / 2) + x / 2] - 128) * YUV2RGB_CONVERT_MATRIX[1, 1] + (yuv420Frames[vIndex + (y / 2) * (width / 2) + x / 2] - 128) * YUV2RGB_CONVERT_MATRIX[1, 2]);
                    rgbFrame[gIndex + y * width + x] = (byte)(temp < 0 ? 0 : (temp > 255 ? 255 : temp));

                    // B
                    temp = (int)(yuv420Frames[y * width + x] + (yuv420Frames[uIndex + (y / 2) * (width / 2) + x / 2] - 128) * YUV2RGB_CONVERT_MATRIX[2, 1]);
                    rgbFrame[bIndex + y * width + x] = (byte)(temp < 0 ? 0 : (temp > 255 ? 255 : temp));
                }
            }
            return rgbFrame;
        }

        /// <summary>
        /// 把YUV420帧中指定位置的YUV像素转换为RGB像素
        /// </summary>
        /// <param name="yuv420">YUV420帧数据</param>
        /// <param name="width">YUV420帧数据宽度</param>
        /// <param name="height">YUV420帧数据高度</param>
        /// <param name="px">像素点X坐标</param>
        /// <param name="py">像素点Y坐标</param>
        /// <returns></returns>
        private static byte[] YUV420ToRGB888(byte[] yuv420, int width, int height, int px, int py)
        {
            int total = width * height;
            byte y, u, v;
            byte[] rgb;
            y = yuv420[py * width + px];
            u = yuv420[(py / 2) * (width / 2) + (px / 2) + total];
            v = yuv420[(py / 2) * (width / 2) + (px / 2) + total + (total / 4)];
            rgb = YUV444ToRGB888(y, u, v);
            return rgb;
        }

        /// <summary>
        /// 把YUV444像素转换为RGB888像素
        /// </summary>
        /// <param name="Y">YUV444像素Y</param>
        /// <param name="U">YUV444像素U</param>
        /// <param name="V">YUV444像素V</param>
        /// <returns></returns>
        private static byte[] YUV444ToRGB888(byte Y, byte U, byte V)
        {
            byte[] rgb = new byte[3];
            int C, D, E;
            byte R, G, B;

            //微软提供转换
            C = Y - 16;
            D = U - 128;
            E = V - 128;

            R = Clip((298 * C + 409 * E + 128) >> 8);
            G = Clip((298 * C - 100 * D - 208 * E + 128) >> 8);
            B = Clip((298 * C + 516 * D + 128) >> 8);

            rgb[0] = R;
            rgb[1] = G;
            rgb[2] = B;

            return rgb;
        }

        private static byte Clip(int p)
        {
            if (p < 0)
            {
                return 0;
            }
            if (p > 255)
            {
                return 255;
            }
            else
            {
                return (byte)p;
            }
        }
    }
}
