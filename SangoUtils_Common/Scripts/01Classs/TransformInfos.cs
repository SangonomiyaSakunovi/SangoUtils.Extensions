using System;

namespace SangoUtils_Common
{
#pragma warning disable CS8618

    [Serializable]
    public class TransformData
    {
        public Vector3Position Vector3Position { get; set; }
        public QuaternionRotation QuaternionRotation { get; set; }
    }

    public class TransformInfo
    {
        public TransformInfo(Vector3Position vector3Position, QuaternionRotation quaternionRotation)
        {
            Vector3Position = vector3Position;
            QuaternionRotation = quaternionRotation;
        }

        public Vector3Position Vector3Position { get; private set; }
        public QuaternionRotation QuaternionRotation { get; private set; }
    }
#pragma warning restore CS8618
}
