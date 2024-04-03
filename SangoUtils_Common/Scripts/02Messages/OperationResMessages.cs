using System;
using System.Collections.Generic;

namespace SangoUtils.Common.Messages
{
    [Serializable]
    public class OperationResReqMessage
    {
        public OperationResReqMessage() { }

        public OperationResReqMessage(uint roomID, OperationRes operationRes)
        {
            RoomID = roomID;
            OperationRes = operationRes;
        }

        public uint RoomID { get; set; } = 0;
        public OperationRes OperationRes { get; set; } = new OperationRes();
    }

    [Serializable]
    public class OperationResEventMessage
    {
        public OperationResEventMessage() { }

        public OperationResEventMessage(uint frameID, List<OperationRes> operationRess)
        {
            FrameID = frameID;
            OperationRes = operationRess;
        }

        public uint FrameID { get; set; } = 0;
        List<OperationRes> OperationRes { get; set; } = new List<OperationRes>();
    }

    [Serializable]
    public class OperationRes
    {
        public OperationRes() { }

        public OperationRes(string entityID_Controller, string entityID_BeControlled, OperationResType operationResType, string operationString)
        {
            EntityID_Controller = entityID_Controller;
            EntityID_BeControlled = entityID_BeControlled;
            OperationResType = operationResType;
            OperationString = operationString;
        }

        public string EntityID_Controller { get; set; } = "";
        public string EntityID_BeControlled { get; set; } = "";
        public OperationResType OperationResType { get; set; } = OperationResType.Default;
        public string OperationString { get; set; } = "";
    }

    [Serializable]
    public enum OperationResType
    {
        Default = 1,
        Move = 2,
        Rotate = 3,
        Zoom = 4,
        Trans = 5,
        ButtonClicked = 6,
        ToggleValueChanged = 7,
    }
}
