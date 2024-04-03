using System;

namespace SangoNetProtocol_Classic
{
    [Serializable]
    public class SangoNetMessage
    {
        public SangoNetMessage() { }

        public SangoNetMessage(NetMessageHead netMessageHead, NetMessageBody netMessageBody, string netMessageTimestamp)
        {
            NetMessageHead = netMessageHead;
            NetMessageBody = netMessageBody;
            NetMessageTimestamp = netMessageTimestamp;
        }

        public NetMessageHead NetMessageHead { get; set; } = new NetMessageHead();
        public NetMessageBody NetMessageBody { get; set; } = new NetMessageBody();
        public string NetMessageTimestamp { get; set; } = "";
    }

    [Serializable]
    public class NetMessageHead
    {
        public NetMessageHead() { }

        public NetMessageHead(int netMessageCommandCode, int netOperationCode)
        {
            NetMessageCommandCode = netMessageCommandCode;
            NetOperationCode = netOperationCode;
        }

        public int NetMessageCommandCode { get; set; } = 1;
        public int NetOperationCode { get; set; } = 1;
    }

    [Serializable]
    public class NetMessageBody
    {
        public NetMessageBody() { }

        public NetMessageBody(int netReturnCode, string netMessageStr)
        {
            NetReturnCode = netReturnCode;
            NetMessageStr = netMessageStr;
        }

        public int NetReturnCode { get; set; } = 1;
        public string NetMessageStr { get; set; } = "";
    }

    //NetMessageCommandCode
        //Default = 1,
        //NetOperationRequest = 2,
        //NetOperationResponse = 3,
        //NetEventData = 4,
        //NetBroadcast = 5,
        //NetUdpMessage = 6

    //NetOperationCode
        //Default = 1,
        //Ping = 2,
        //Login = 3,
        //Regist = 4,
        //Select = 5,
        //LoadResource = 6,
        //Aoi = 7,
        //OperationKey = 8,
        //OperationResult = 9
        //AnchorUpload = 10,

    //NetReturnCode
        //Default = 1,
        //Succeed = 2,
        //Failed = 3

    //NetErrorCode
        //UnKnown_Error = 1,
        //Server_Data_Error = 2
}
