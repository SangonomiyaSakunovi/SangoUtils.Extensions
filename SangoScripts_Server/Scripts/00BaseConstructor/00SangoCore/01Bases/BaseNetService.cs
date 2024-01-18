using SangoNetProtol;
using SangoUtils_Logger;

namespace SangoUtils_Server_Scripts
{
    public abstract class BaseNetService<T> : BaseService<T> where T : class, new()
    {
        protected Dictionary<NetOperationCode, BaseNetHandler> _netHandlerDict = new();
        protected Dictionary<NetOperationCode, BaseNetController> _netControllerDict = new();

        public abstract void SendOperationResponse(NetOperationCode operationCode, string messageStr);

        public abstract void SendOperationResponse(NetOperationCode operationCode, NetReturnCode returnCode);

        public abstract void SendEvent(NetOperationCode operationCode, string messageStr);

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

        public K GetNetHandler<K>(NetOperationCode operationCode) where K : BaseNetHandler, new()
        {
            if (_netHandlerDict.ContainsKey(operationCode))
            {
                return (K)_netHandlerDict[operationCode];
            }
            else
            {
                K netHandler = new();
                netHandler.OnInit<T>(operationCode, this);
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

        public K GetNetController<K>(NetOperationCode operationCode) where K : BaseNetController, new()
        {
            if (_netControllerDict.ContainsKey(operationCode))
            {
                return (K)_netControllerDict[operationCode];
            }
            else
            {
                K netController = new();
                netController.OnInit<T>(operationCode, this);
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

    }
}
