using System;
using System.Collections.Generic;

namespace SangoUtils.Common.Messages
{
    [Serializable]
    public class OperationKeyReqMessage
    {
        public OperationKeyReqMessage() { }

        public OperationKeyReqMessage(uint roomID, OperationKey operationKey)
        {
            RoomID = roomID;
            OperationKey = operationKey;
        }

        public uint RoomID { get; set; } = 0;
        public OperationKey OperationKey { get; set; } = new OperationKey();
    }

    [Serializable]
    public class OperationKeyEventMessage
    {
        public OperationKeyEventMessage() { }

        public OperationKeyEventMessage(uint frameID, List<OperationKey> operationKeys)
        {
            FrameID = frameID;
            OperationKeys = operationKeys;
        }

        public uint FrameID { get; set; } = 0;
        List<OperationKey> OperationKeys { get; set; } = new List<OperationKey>();
    }
    [Serializable]
    public class OperationKey
    {
        public OperationKey() { }

        public OperationKey(string entityID, OperationKeyType operationKeyType, string operationString)
        {
            EntityID = entityID;
            OperationKeyType = operationKeyType;
            OperationString = operationString;
        }

        public string EntityID { get; set; } = "";
        public OperationKeyType OperationKeyType { get; set; } = OperationKeyType.None;
        public string OperationString { get; set; } = "";
    }

    [Serializable]
    public enum OperationKeyType
    {
        None,
        Move,
        Rotate,
        Zoom,
    }
}
