using SangoNetProtol;
using SangoScripts_Server.IOCP;
using SangoScripts_Server.Logger;

namespace SangoScripts_Server.Net
{
    public class NetService : BaseService<NetService>
    {
        private IOCPPeer<ClientPeer>? _serverPeerInstance;

        private Dictionary<NetOperationCode, BaseNetHandler> _netHandlerDict = new();
        private Dictionary<NetOperationCode, BaseNetController> _netControllerDict = new();

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

        public void SendOperationResponse(NetOperationCode operationCode, string messageStr)
        {
            _serverPeerInstance?.ClientPeer?.SendOperationResponse(operationCode, messageStr);
        }

        public void SendOperationResponse(NetOperationCode operationCode, NetReturnCode returnCode)
        {
            _serverPeerInstance?.ClientPeer?.SendOperationResponse(operationCode, returnCode);
        }

        public void SendEvent(NetOperationCode operationCode, string messageStr)
        {
            _serverPeerInstance?.ClientPeer?.SendEvent(operationCode, messageStr);
        }

        public void NetRequestMessageBroadcast(SangoNetMessage sangoNetMessage, ClientPeer peer)
        {
            if (_netHandlerDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetHandler? netHandler))
            {
                netHandler.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
            else
            {
                _netHandlerDict.TryGetValue(NetOperationCode.Default, out BaseNetHandler? defaultNetHandle);
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
                SangoLogger.Error("Already has this NetHandler.");
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
                SangoLogger.Error("Already remove this NetHandler.");
            }
        }

        public void AddNetController(BaseNetController netController)
        {
            if (!_netControllerDict.ContainsKey(netController.NetOperationCode))
            {
                _netControllerDict.Add(netController.NetOperationCode, netController);
            }
            else
            {
                SangoLogger.Error("Already has this NetController.");
            }
        }

        public T GetNetController<T>(NetOperationCode operationCode) where T : BaseNetController, new()
        {
            if (_netControllerDict.ContainsKey(operationCode))
            {
                return (T)_netControllerDict[operationCode];
            }
            else
            {
                T netController = new();
                netController.OnInit(operationCode);
                return netController;
            }
        }

        public void RemoveNetController(BaseNetController netController)
        {
            if (_netControllerDict.ContainsKey(netController.NetOperationCode))
            {
                _netControllerDict.Remove(netController.NetOperationCode);
            }
            else
            {
                SangoLogger.Error("Already remove this NetController.");
            }
        }

        private void InitClientInstance(string ipAddress, int port, int maxConnectCount)
        {
            _serverPeerInstance = new IOCPPeer<ClientPeer>();
            _serverPeerInstance.InitAsServer(ipAddress, port, maxConnectCount);
        }

        public void CloseClientInstance()
        {
            _serverPeerInstance?.CloseServer();
        }
    }
}
