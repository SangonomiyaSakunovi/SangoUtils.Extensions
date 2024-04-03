using SangoNetProtocol_Classic;

namespace SangoUtils.NetOperationClassic
{
    public interface IUdpClientOperation
    {
        public void OpenClient();

        public void CloseClient();

        public void SendOperationUdpMessage(int operationCode, string messageStr);

        public void OnMessageReceived(SangoNetMessage sangoNetMessage);

        public T GetNetUdpMessage<T>(int operationCode) where T : BaseNetUdpMessage, new();
    }
}
