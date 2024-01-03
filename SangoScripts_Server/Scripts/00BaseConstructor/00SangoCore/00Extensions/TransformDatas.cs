using System.Numerics;

namespace SangoScripts_Server
{
    public class Transform
    {
        public Transform() { }

        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public Vector3 Position { get; set; } = new Vector3(0, 0, 0);
        public Quaternion Rotation { get; set; } = new Quaternion(0, 0, 0, 0);
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);
    }
}
