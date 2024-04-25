using System;

namespace SangoUtils.Commons.Messages
{
    [Serializable]
    public class TransformFixedMessage
    {
        public TransformFixedMessage() { }

        public TransformFixedMessage(Vector3FixedMessage position, QuaternionFixedMessage rotation, Vector3FixedMessage scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3FixedMessage Position { get; set; } = new Vector3FixedMessage(0, 0, 0);
        public QuaternionFixedMessage Rotation { get; set; } = new QuaternionFixedMessage(0, 0, 0, 0);
        public Vector3FixedMessage Scale { get; set; } = new Vector3FixedMessage(1, 1, 1);
    }

    [Serializable]
    public class Vector3FixedMessage
    {
        public Vector3FixedMessage() { }

        public Vector3FixedMessage(long x, long y, long z)
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
    public class QuaternionFixedMessage
    {
        public QuaternionFixedMessage() { }

        public QuaternionFixedMessage(long x, long y, long z, long w)
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
