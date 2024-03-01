using System;

namespace SangoUtils_Common_Classic.Messages
{
    [Serializable]
    public class TransformFloatMessage
    {
        public TransformFloatMessage() { }

        public TransformFloatMessage(Vector3FloatMessage position, QuaternionFloatMessage rotation, Vector3FloatMessage scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3FloatMessage Position { get; set; } = new Vector3FloatMessage(0, 0, 0);
        public QuaternionFloatMessage Rotation { get; set; } = new QuaternionFloatMessage(0, 0, 0, 0);
        public Vector3FloatMessage Scale { get; set; } = new Vector3FloatMessage(1, 1, 1);
    }

    [Serializable]
    public class Vector3FloatMessage
    {
        public Vector3FloatMessage() { }

        public Vector3FloatMessage(float x, float y, float z)
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
    public class QuaternionFloatMessage
    {
        public QuaternionFloatMessage() { }

        public QuaternionFloatMessage(float x, float y, float z, float w)
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
