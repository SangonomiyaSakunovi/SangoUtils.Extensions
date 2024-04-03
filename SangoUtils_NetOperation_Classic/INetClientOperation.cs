using SangoNetProtocol_Classic;

namespace SangoUtils.NetOperationClassic
{
    public interface INetClientOperation
    {
        public void OpenClient();

        public void CloseClient();

        public void SendOperationRequest(int operationCode, string messageStr);

        public void SendOperationBroadcast(int operationCode, string messageStr);

        public void OnMessageReceived(SangoNetMessage sangoNetMessage);

        public T GetNetRequest<T>(int netOperationCode) where T : BaseNetRequest, new();

        public T GetNetEvent<T>(int operationCode) where T : BaseNetEvent, new();

        public T GetNetBroadcast<T>(int operationCode) where T : BaseNetBroadcast, new();
    }
}
