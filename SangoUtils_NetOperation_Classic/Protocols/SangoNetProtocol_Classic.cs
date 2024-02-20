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

        public NetMessageHead(NetMessageCommandCode netMessageCommandCode, NetOperationCode netOperationCode)
        {
            NetMessageCommandCode = netMessageCommandCode;
            NetOperationCode = netOperationCode;
        }

        public NetMessageCommandCode NetMessageCommandCode { get; set; } = NetMessageCommandCode.Default;
        public NetOperationCode NetOperationCode { get; set; } = NetOperationCode.Default;
    }

    [Serializable]
    public class NetMessageBody
    {
        public NetMessageBody() { }

        public NetMessageBody(NetReturnCode netReturnCode, string netMessageStr)
        {
            NetReturnCode = netReturnCode;
            NetMessageStr = netMessageStr;
        }

        public NetReturnCode NetReturnCode { get; set; } = NetReturnCode.Default;
        public string NetMessageStr { get; set; } = "";
    }

    [Serializable]
    public enum NetMessageCommandCode
    {
        Default = 1,
        NetOperationRequest = 2,
        NetOperationResponse = 3,
        NetEventData = 4,
        NetBroadcast = 5,
        NetUdpMessage = 6
    }

    [Serializable]
    public enum NetOperationCode
    {
        Default = 1,
        Ping = 2,
        Login = 3,
        Regist = 4,
        Select = 5,
        LoadResource = 6,
        Aoi = 7,
        OperationKey = 8,
        OperationResult = 9
    }

    [Serializable]
    public enum NetReturnCode
    {
        Default = 1,
        Succeed = 2,
        Failed = 3
    }

    [Serializable]
    public enum NetErrorCode
    {
        UnKnown_Error = 1,
        Server_Data_Error = 2
    }
}
