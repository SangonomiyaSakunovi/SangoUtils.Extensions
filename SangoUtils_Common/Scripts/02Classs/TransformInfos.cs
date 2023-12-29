using System;
using System.Numerics;

namespace SangoUtils_Common
{
#pragma warning disable CS8618

    [Serializable]
    public class TransformInfo
    {
        public TransformInfo(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }

    [Serializable]
    public class SyncTransformInfo
    {
        public SyncTransformInfo(string entityID, TransformInfo transformInfo)
        {
            EntityID = entityID;
            TransformInfo = transformInfo;
        }

        public string EntityID { get; set; }
        public TransformInfo TransformInfo { get; set; }
    }
#pragma warning restore CS8618
}
