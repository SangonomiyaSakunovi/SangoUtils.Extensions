using System;

namespace SangoUtils_Common.Infos
{
    [Serializable]
    public class TransformInfo
    {
        public TransformInfo() { }

        public TransformInfo(Vector3Info position, QuaternionInfo rotation, Vector3Info scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3Info Position { get; set; } = new Vector3Info(0, 0, 0);
        public QuaternionInfo Rotation { get; set; } = new QuaternionInfo(0, 0, 0, 0);
        public Vector3Info Scale { get; set; } = new Vector3Info(1, 1, 1);
    }

    [Serializable]
    public class Vector3Info
    {
        public Vector3Info() { }

        public Vector3Info(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;
    }

    [Serializable]
    public class QuaternionInfo
    {
        public QuaternionInfo() { }

        public QuaternionInfo(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float X { get; set; } = 0;
        public float Y { get; set; } = 0;
        public float Z { get; set; } = 0;
        public float W { get; set; } = 0;
    }
}
