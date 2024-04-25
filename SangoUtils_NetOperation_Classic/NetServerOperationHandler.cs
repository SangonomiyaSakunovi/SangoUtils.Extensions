using System;
using System.Collections.Generic;

namespace SangoUtils.NetOperations
{
    public class NetServerOperationHandler
    {
        private readonly Dictionary<int, BaseNetHandler> _netHandlerDict = new Dictionary<int, BaseNetHandler>();
        private readonly Dictionary<int, BaseNetController> _netControllerDict = new Dictionary<int, BaseNetController>();

        public void NetMessageCommandBroadcast(NetOpMessage message, BaseNetClientPeer peer)
        {
            switch (message.OpCMD)
            {
                case 1:
                    {
                        NetRequestMessageBroadcast(message, peer);
                    }
                    break;
            }
        }

        private void NetRequestMessageBroadcast(NetOpMessage message, BaseNetClientPeer peer)
        {
            if (_netHandlerDict.TryGetValue(message.OpCode, out BaseNetHandler? netHandler))
            {
                netHandler.OnOperationRequest(message.Message, peer);
            }
            else
            {
                _netHandlerDict.TryGetValue(0, out BaseNetHandler? defaultNetHandler);
                defaultNetHandler?.OnOperationRequest(message.Message, peer);
            }
        }

        public void AddNetHandler(BaseNetHandler netHandler)
        {
            if (!_netHandlerDict.ContainsKey(netHandler.OpCode))
            {
                _netHandlerDict.Add(netHandler.OpCode, netHandler);
            }
            else
            {
            }
        }

        public T GetNetHandler<T>(int operationCode) where T : BaseNetHandler, new()
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
            if (_netHandlerDict.ContainsKey(netHandler.OpCode))
            {
                _netHandlerDict.Remove(netHandler.OpCode);
            }
            else
            {
            }
        }

        public void AddNetController(BaseNetController netController)
        {
            if (!_netControllerDict.ContainsKey(netController.OpCode))
            {
                _netControllerDict.Add(netController.OpCode, netController);
            }
            else
            {
            }
        }

        public T GetNetController<T>(int operationCode) where T : BaseNetController, new()
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
            if (_netControllerDict.ContainsKey(netController.OpCode))
            {
                _netControllerDict.Remove(netController.OpCode);
            }
            else
            {
            }
        }
    }
}
