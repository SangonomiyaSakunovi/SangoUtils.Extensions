using System;
using System.Collections.Generic;

namespace SangoUtils.CommonClassic.Messages
{
    [Serializable]
    public class OperationResReqMessage
    {
        public OperationResReqMessage() { }

        public OperationResReqMessage(OperationRes operationRes)
        {
            OperationRes = operationRes;
        }

        public OperationRes OperationRes { get; set; } = new OperationRes();
    }

    [Serializable]
    public class OperationResEventMessage
    {
        public OperationResEventMessage() { }

        public OperationResEventMessage(List<OperationRes> operationRess)
        {
            OperationRes = operationRess;
        }

        List<OperationRes> OperationRes { get; set; } = new List<OperationRes>();
    }

    [Serializable]
    public class OperationRes
    {
        public OperationRes() { }

        public OperationRes(string entityID_Controller, string entityID_BeControlled, int operationResType, string operationString)
        {
            EntityID_Controller = entityID_Controller;
            EntityID_BeControlled = entityID_BeControlled;
            OperationResType = operationResType;
            OperationString = operationString;
        }

        public string EntityID_Controller { get; set; } = "";
        public string EntityID_BeControlled { get; set; } = "";
        public int OperationResType { get; set; } = 1;
        public string OperationString { get; set; } = "";
    }

    //OperationResType
    //Default = 1,
    //Move = 2,
    //Rotate = 3,
    //Zoom = 4,
    //Trans = 5,
    //ButtonClicked = 6,
    //ToggleValueChanged = 7,
}
