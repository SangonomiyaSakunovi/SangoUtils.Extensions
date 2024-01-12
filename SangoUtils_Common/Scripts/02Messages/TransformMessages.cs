using System;

namespace SangoUtils_Common.Messages
{
    [Serializable]
    public class TransformMessage
    {
        public TransformMessage() { }

        public TransformMessage(Vector3Message position, QuaternionMessage rotation, Vector3Message scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3Message Position { get; set; } = new Vector3Message(0, 0, 0);
        public QuaternionMessage Rotation { get; set; } = new QuaternionMessage(0, 0, 0, 0);
        public Vector3Message Scale { get; set; } = new Vector3Message(1, 1, 1);
    }

    [Serializable]
    public class Vector3Message
    {
        public Vector3Message() { }

        public Vector3Message(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public long X { get; set; } = 0;
        public long Y { get; set; } = 0;
        public long Z { get; set; } = 0;
    }

    [Serializable]
    public class QuaternionMessage
    {
        public QuaternionMessage() { }

        public QuaternionMessage(long x, long y, long z, long w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public long X { get; set; } = 0;
        public long Y { get; set; } = 0;
        public long Z { get; set; } = 0;
        public long W { get; set; } = 0;
    }
}
