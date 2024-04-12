namespace SangoUtils.NetOperation
{
    public interface INetClientOperation
    {
        public void OpenClient();

        public void CloseClient();

        public void SendRequest(int opCode, string message);

        public void OnMessage(NetOpMessage message);

        public T GetNetRequest<T>(int opCode) where T : BaseNetRequest, new();

        public T GetNetEvent<T>(int opCode) where T : BaseNetEvent, new();
    }
}
