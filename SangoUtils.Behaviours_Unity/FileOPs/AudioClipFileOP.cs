using System;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.FileOPs
{
    public static class AudioClipFileOP
    {
        public static byte[] GetRealAudio(ref AudioClip recordedClip)
        {
            int position = Microphone.GetPosition(null);
            if (position <= 0 || position > recordedClip.samples)
            {
                position = recordedClip.samples;
            }
            float[] soundata = new float[position * recordedClip.channels];
            recordedClip.GetData(soundata, 0);
            recordedClip = AudioClip.Create(recordedClip.name, position,
            recordedClip.channels, recordedClip.frequency, false);
            recordedClip.SetData(soundata, 0);
            int rescaleFactor = 32767;
            byte[] outData = new byte[soundata.Length * 2];
            for (int i = 0; i < soundata.Length; i++)
            {
                short temshort = (short)(soundata[i] * rescaleFactor);
                byte[] temdata = BitConverter.GetBytes(temshort);
                outData[i * 2] = temdata[0];
                outData[i * 2 + 1] = temdata[1];
            }
            return outData;
        }
    }
}
