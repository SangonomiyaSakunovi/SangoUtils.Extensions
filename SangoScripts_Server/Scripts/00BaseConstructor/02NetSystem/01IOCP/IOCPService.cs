using SangoNetProtol;
using SangoUtils_IOCP;
using SangoUtils_Logger;

namespace SangoUtils_Server_Scripts.Net
{
    public class IOCPService : BaseNetService<IOCPService>
    {
        private IOCPPeer<IOCPClientPeer>? _serverPeerInstance;

        public override void OnInit()
        {
            base.OnInit();

            string iPAddress = "192.168.3.55";
            int port = 52517;
            int maxConnectCount = 10000;
            InitClientInstance(iPAddress, port, maxConnectCount);
            SangoLogger.Done("SangoServer is Run!");

            DefaultIOCPHandler defaultNetHandler = GetNetHandler<DefaultIOCPHandler>(NetOperationCode.Default);
        }

        public override void SendOperationResponse(NetOperationCode operationCode, string messageStr)
        {
            _serverPeerInstance?.ClientPeer?.SendOperationResponse(operationCode, messageStr);
        }

        public override void SendOperationResponse(NetOperationCode operationCode, NetReturnCode returnCode)
        {
            _serverPeerInstance?.ClientPeer?.SendOperationResponse(operationCode, returnCode);
        }

        public override void SendEvent(NetOperationCode operationCode, string messageStr)
        {
            _serverPeerInstance?.ClientPeer?.SendEvent(operationCode, messageStr);
        }

        public void OnMessageReceived(SangoNetMessage sangoNetMessage, IOCPClientPeer peer)
        {
            NetMessageCommandBroadcast(sangoNetMessage, peer);
        }

        protected void NetMessageCommandBroadcast(SangoNetMessage sangoNetMessage, IOCPClientPeer peer)
        {
            switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
            {
                case NetMessageCommandCode.NetOperationRequest:
                    NetRequestMessageBroadcast(sangoNetMessage, peer);
                    break;
            }
        }

        private void NetRequestMessageBroadcast(SangoNetMessage sangoNetMessage, IOCPClientPeer peer)
        {
            if (_netHandlerDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetHandler? netHandler))
            {
                BaseIOCPNetHandler? handler = netHandler as BaseIOCPNetHandler;
                handler?.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
            else
            {
                _netHandlerDict.TryGetValue(NetOperationCode.Default, out BaseNetHandler? defaultNetHandle);
                BaseIOCPNetHandler? handler = netHandler as BaseIOCPNetHandler;
                handler?.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
        }

        private void InitClientInstance(string ipAddress, int port, int maxConnectCount)
        {
            _serverPeerInstance = new IOCPPeer<IOCPClientPeer>();
            _serverPeerInstance.OpenAsServer(ipAddress, port, maxConnectCount);
        }

        public void CloseClientInstance()
        {
            _serverPeerInstance?.CloseAsServer();
        }
    }
}
