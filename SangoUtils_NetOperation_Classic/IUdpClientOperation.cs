using SangoNetProtocol_Classic;

namespace SangoUtils_NetOperation_Classic
{
    public interface IUdpClientOperation
    {
        public void OpenClient();

        public void CloseClient();

        public void SendOperationUdpMessage(NetOperationCode operationCode, string messageStr);

        public void OnMessageReceived(SangoNetMessage sangoNetMessage);

        public T GetNetUdpMessage<T>(NetOperationCode operationCode) where T : BaseNetUdpMessage, new();
    }
}
