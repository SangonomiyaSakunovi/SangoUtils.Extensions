using SangoNetProtol;
using SangoUtils_Common.Utils;
using SangoUtils_Server.IOCP;

namespace SangoUtils_Server.Net
{
    public class ClientPeer : IClientPeer_IOCP
    {
        #region private
        private string _uid = "";
        private long _lastMessageTimestamp = long.MinValue;
        #endregion

        public string UID { get { return _uid; } }

        protected override void OnConnected()
        {
            IOCPLogger.Info("A new client is Connected.");
        }

        protected override void OnDisconnected()
        {
            IOCPLogger.Info("A client is DisConnected.");
        }

        protected override void OnReceivedMessage(byte[] byteMessages)
        {
            SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(byteMessages);
            long messageTimestamp = Convert.ToInt64(sangoNetMessage.NetMessageTimestamp);
            if (messageTimestamp > _lastMessageTimestamp)
            {
                _lastMessageTimestamp = messageTimestamp;
                switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
                {
                    case NetMessageCommandCode.NetOperationRequest:
                        NetService.Instance.NetRequestMessageBroadcast(sangoNetMessage, this);
                        break;
                }
            }
        }

        public void SendOperationResponse(NetOperationCode operationCode, string messageStr)
        {
            NetMessageHead messageHead = new()
            {
                NetOperationCode = operationCode,
                NetMessageCommandCode = NetMessageCommandCode.NetOperationResponse
            };
            NetMessageBody messageBody = new()
            {
                NetMessageStr = messageStr
            };
            SangoNetMessage message = new()
            {
                NetMessageHead = messageHead,
                NetMessageBody = messageBody,
                NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
            };
            SendData(message);
        }

        public void SendOperationResponse(NetOperationCode operationCode, NetReturnCode returnCode)
        {
            NetMessageHead messageHead = new()
            {
                NetOperationCode = operationCode,
                NetMessageCommandCode = NetMessageCommandCode.NetOperationResponse
            };
            NetMessageBody messageBody = new()
            {
                NetReturnCode = returnCode
            };
            SangoNetMessage message = new()
            {
                NetMessageHead = messageHead,
                NetMessageBody = messageBody,
                NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
            };
            SendData(message);
        }

        public void SendEvent(NetOperationCode operationCode, string messageStr)
        {
            NetMessageHead messageHead = new()
            {
                NetOperationCode = operationCode,
                NetMessageCommandCode = NetMessageCommandCode.NetEventData
            };
            NetMessageBody messageBody = new()
            {
                NetMessageStr = messageStr
            };
            SangoNetMessage message = new()
            {
                NetMessageHead = messageHead,
                NetMessageBody = messageBody,
                NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
            };
            SendData(message);
        }

        private void SendData(SangoNetMessage message)
        {
            byte[] bytes = ProtoUtils.SetProtoBytes(message);
            SendMessage(bytes);
        }
    }
}
