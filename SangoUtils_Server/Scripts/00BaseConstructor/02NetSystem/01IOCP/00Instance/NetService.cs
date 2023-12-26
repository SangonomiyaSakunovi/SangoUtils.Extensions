using SangoNetProtol;
using SangoUtils_Server.Core;
using SangoUtils_Server.IOCP;
using SangoUtils_Server.Logger;

namespace SangoUtils_Server.Net
{
    public class NetService : BaseService<NetService>
    {
        public IOCPPeer<ClientPeer>? ServerPeerInstance;

        private Dictionary<NetOperationCode, BaseNetHandler> _netHandlerDict = new Dictionary<NetOperationCode, BaseNetHandler>();

        public override void OnInit()
        {
            base.OnInit();

            string iPAddress = "127.0.0.1";
            int port = 52037;
            int maxConnectCount = 10000;
            InitClientInstance(iPAddress, port, maxConnectCount);
            SangoLogger.Done("SangoServer is Run!");

            DefaultNetHandler defaultNetHandler = GetNetHandler<DefaultNetHandler>(NetOperationCode.Default);
        }

        public void NetRequestMessageBroadcast(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            if (_netHandlerDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetHandler? netHandler))
            {
                netHandler.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
            else
            {
                _netHandlerDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetHandler? defaultNetHandle);
                defaultNetHandle?.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
        }

        public void AddNetHandler(BaseNetHandler netHandler)
        {
            if (!_netHandlerDict.ContainsKey(netHandler.NetOperationCode))
            {
                _netHandlerDict.Add(netHandler.NetOperationCode, netHandler);
            }
            else
            {
                SangoLogger.Error("Already has this handler.");
            }
        }

        public T GetNetHandler<T>(NetOperationCode operationCode) where T : BaseNetHandler, new()
        {
            if (_netHandlerDict.ContainsKey(operationCode))
            {
                return (T)_netHandlerDict[operationCode];
            }
            else
            {
                T netHandler = new();
                netHandler.OnInit(operationCode);
                return netHandler;
            }
        }

        public void RemoveNetHandler(BaseNetHandler netHandler)
        {
            if (_netHandlerDict.ContainsKey(netHandler.NetOperationCode))
            {
                _netHandlerDict.Remove(netHandler.NetOperationCode);
            }
            else
            {
                SangoLogger.Error("Already remove this handler.");
            }
        }

        private void InitClientInstance(string ipAddress, int port, int maxConnectCount)
        {
            ServerPeerInstance = new IOCPPeer<ClientPeer>();
            ServerPeerInstance.InitAsServer(ipAddress, port, maxConnectCount);
        }

        public void CloseClientInstance()
        {
            ServerPeerInstance?.CloseServer();
        }
    }
}
