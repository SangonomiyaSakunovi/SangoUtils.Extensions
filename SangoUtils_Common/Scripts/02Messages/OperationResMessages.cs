using System;
using System.Collections.Generic;

namespace SangoUtils_Common.Messages
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
        public OperationResType OperationResType { get; set; } = OperationResType.None;
        public string OperationString { get; set; } = "";
    }

    [Serializable]
    public enum OperationResType
    {
        None,
        Move,
        Rotate,
        Zoom,
        Trans,
        ButtonClicked,
        ToggleValueChanged,
    }
}
