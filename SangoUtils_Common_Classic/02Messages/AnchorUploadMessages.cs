using System;

namespace SangoUtils.CommonsClassic.Messages
{
    [Serializable]
    public class AnchorUploadMessage
    {
        public AnchorUploadMessage() { }

        public AnchorUploadMessage(string anchorID, int anchorUploadedType, Vector3FloatMessage position, QuaternionFloatMessage rotation, Vector3FloatMessage scale)
        {
            AnchorID = anchorID;
            AnchorUploadedType = anchorUploadedType;
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public string AnchorID { get; set; } = "";
        public int AnchorUploadedType { get; set; } = 1;
        public Vector3FloatMessage Position { get; set; } = new Vector3FloatMessage(0, 0, 0);
        public QuaternionFloatMessage Rotation { get; set; } = new QuaternionFloatMessage(0, 0, 0, 0);
        public Vector3FloatMessage Scale { get; set; } = new Vector3FloatMessage(1, 1, 1);
    }

    //AnchorUploadedType
    //Default = 1,
    //HolographicSpace = 2,
    //PersistedObject = 3
}
