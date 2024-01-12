using SangoUtils_Common.Messages;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace SangoScripts_Server
{
    public struct TransformData
    {
        public TransformData() { }

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public TransformData(TransformMessage transform)
        {
            Position = new(transform.Position.X, transform.Position.Y, transform.Position.Z);
            Rotation = new(transform.Rotation.X, transform.Rotation.Y, transform.Rotation.Z, transform.Rotation.W);
            Scale = new(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
        }

        public Vector3 Position { get; set; } = new(0, 0, 0);
        public Quaternion Rotation { get; set; } = new(0, 0, 0, 0);
        public Vector3 Scale { get; set; } = new(1, 1, 1);

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is TransformData otherTrans && this.Equals(otherTrans);

        public readonly bool Equals(TransformData otherTrans) => Position.Equals(otherTrans.Position) && Rotation.Equals(otherTrans.Rotation) && Scale.Equals(otherTrans.Scale);

        public override readonly int GetHashCode() => (Position, Rotation, Scale).GetHashCode();

        public static bool operator ==(TransformData left, TransformData right) => left.Equals(right);

        public static bool operator !=(TransformData left, TransformData right) => !(left == right);
    }
}
