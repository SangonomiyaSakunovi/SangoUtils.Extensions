using UnityEngine;

namespace SangoUtils.Extensions_Unity
{
    public struct TransformData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public TransformData(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
            Scale = transform.localScale;
        }
        public TransformData(GameObject gameObject)
        {
            Position = gameObject.transform.position;
            Rotation = gameObject.transform.rotation;
            Scale = gameObject.transform.localScale;
        }
        public override readonly bool Equals(object obj)
        {
            return obj is TransformData data &&
                   Position.Equals(data.Position) &&
                   Rotation.Equals(data.Rotation) &&
                   Scale.Equals(data.Scale);
        }
        public override readonly int GetHashCode()
        {
            int hashCode = -1568518017;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + Scale.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(TransformData left, TransformData right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(TransformData left, TransformData right)
        {
            return !(left == right);
        }
        public static implicit operator TransformData(Transform transform)
        {
            return new TransformData(transform);
        }
        public static implicit operator TransformData(GameObject gameObject)
        {
            return new TransformData(gameObject);
        }
        public override readonly string ToString()
        {
            return $"Position: {Position.ToString()}, Rotation: {Rotation.ToString()}, Scale: {Scale.ToString()}";
        }
    }
}
