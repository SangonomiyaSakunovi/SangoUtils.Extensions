using UnityEngine;

namespace SangoUtils.Behaviours_Unity.Structs
{
    public struct TransData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public TransData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public TransData(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
            Scale = transform.localScale;
        }
        public TransData(GameObject gameObject)
        {
            Position = gameObject.transform.position;
            Rotation = gameObject.transform.rotation;
            Scale = gameObject.transform.localScale;
        }
        public override readonly bool Equals(object obj)
        {
            return obj is TransData data &&
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
        public static bool operator ==(TransData left, TransData right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(TransData left, TransData right)
        {
            return !(left == right);
        }
        public static implicit operator TransData(Transform transform)
        {
            return new TransData(transform);
        }
        public static implicit operator TransData(GameObject gameObject)
        {
            return new TransData(gameObject);
        }
        public override readonly string ToString()
        {
            return $"Position: {Position.ToString()}, Rotation: {Rotation.ToString()}, Scale: {Scale.ToString()}";
        }
    }
}
