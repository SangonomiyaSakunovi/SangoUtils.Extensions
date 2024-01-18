using SangoNetProtol;
using System;
using System.Collections.Generic;

namespace SangoUtils_NetOperation
{
    public class NetServerOperationHandler
    {
        private readonly Dictionary<NetOperationCode, BaseNetHandler> _netHandlerDict = new Dictionary<NetOperationCode, BaseNetHandler>();
        private readonly Dictionary<NetOperationCode, BaseNetController> _netControllerDict = new Dictionary<NetOperationCode, BaseNetController>();
        private readonly Dictionary<NetOperationCode, BaseNetClientBroadcast> _netClientBroadcastDict = new Dictionary<NetOperationCode, BaseNetClientBroadcast>();

        public void NetMessageCommandBroadcast(SangoNetMessage sangoNetMessage, BaseNetClientPeer peer)
        {
            switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
            {
                case NetMessageCommandCode.NetOperationRequest:
                    {
                        NetRequestMessageBroadcast(sangoNetMessage, peer);
                    }
                    break;
                case NetMessageCommandCode.NetBroadcast:
                    {
                        NetMessageBroadcastBroadcast(sangoNetMessage, peer);
                    }
                    break;
            }
        }

        private void NetRequestMessageBroadcast(SangoNetMessage sangoNetMessage, BaseNetClientPeer peer)
        {
            if (_netHandlerDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetHandler? netHandler))
            {
                netHandler.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
            else
            {
                _netHandlerDict.TryGetValue(NetOperationCode.Default, out BaseNetHandler? defaultNetHandler);
                defaultNetHandler?.OnOperationRequest(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
        }

        private void NetMessageBroadcastBroadcast(SangoNetMessage sangoNetMessage, BaseNetClientPeer peer)
        {
            if (_netClientBroadcastDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetClientBroadcast? netClientBroadcast))
            {
                netClientBroadcast.OnOperationClientBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
            }
            else
            {
                _netClientBroadcastDict.TryGetValue(NetOperationCode.Default, out BaseNetClientBroadcast? defaultNetClientBroadcast);
                defaultNetClientBroadcast?.OnOperationClientBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr, peer);
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
                T netHandler = Activator.CreateInstance<T>();
                netHandler.OnInit(operationCode, this);
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
                T netController = Activator.CreateInstance<T>();
                netController.OnInit(operationCode, this);
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
            }
        }

        public void AddNetClientBroadcast(BaseNetClientBroadcast netClientBroadcast)
        {
            if (!_netControllerDict.ContainsKey(netClientBroadcast.NetOperationCode))
            {
                _netClientBroadcastDict.Add(netClientBroadcast.NetOperationCode, netClientBroadcast);
            }
            else
            {
            }
        }

        public T GetNetClientBroadcast<T>(NetOperationCode operationCode) where T : BaseNetClientBroadcast, new()
        {
            if (_netClientBroadcastDict.ContainsKey(operationCode))
            {
                return (T)_netClientBroadcastDict[operationCode];
            }
            else
            {
                T netController = Activator.CreateInstance<T>();
                netController.OnInit(operationCode, this);
                return netController;
            }
        }

        public void RemoveNetClientBroadcast(BaseNetClientBroadcast netClientBroadcast)
        {
            if (_netClientBroadcastDict.ContainsKey(netClientBroadcast.NetOperationCode))
            {
                _netClientBroadcastDict.Remove(netClientBroadcast.NetOperationCode);
            }
            else
            {
            }
        }
    }
}
