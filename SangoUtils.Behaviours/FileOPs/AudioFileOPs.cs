using System;
using System.IO;

namespace SangoUtils.Behaviours.FileOPs
{
    public static class WavFileOP
    {
        public static void WriteWavHeader(FileStream stream, int hz, int channels, int samples)
        {
            stream.Seek(0, SeekOrigin.Begin);

            byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            stream.Write(riff, 0, 4);

            byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
            stream.Write(chunkSize, 0, 4);

            byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            stream.Write(wave, 0, 4);

            byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            stream.Write(fmt, 0, 4);

            byte[] subChunk1 = BitConverter.GetBytes(16);
            stream.Write(subChunk1, 0, 4);

            ushort one = 1;
            //UInt16 two = 2;

            byte[] audioFormat = BitConverter.GetBytes(one);
            stream.Write(audioFormat, 0, 2);

            byte[] numChannels = BitConverter.GetBytes(channels);
            stream.Write(numChannels, 0, 2);

            byte[] sampleRate = BitConverter.GetBytes(hz);
            stream.Write(sampleRate, 0, 4);

            byte[] byteRate = BitConverter.GetBytes(hz * channels * 2);
            stream.Write(byteRate, 0, 4);

            ushort blockAlign = (ushort)(channels * 2);
            stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            ushort bps = 16;
            byte[] bitsPerSample = BitConverter.GetBytes(bps);
            stream.Write(bitsPerSample, 0, 2);

            byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
            stream.Write(datastring, 0, 4);

            byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
            stream.Write(subChunk2, 0, 4);
        }

        public static FileStream CreateEmpty(string filepath)
        {
            FileStream fileStream = new FileStream(filepath, FileMode.Create);
            byte emptyByte = new byte();

            for (int i = 0; i < 44; i++) //为wav文件头留出空间
            {
                fileStream.WriteByte(emptyByte);
            }

            return fileStream;
        }
    }
}
