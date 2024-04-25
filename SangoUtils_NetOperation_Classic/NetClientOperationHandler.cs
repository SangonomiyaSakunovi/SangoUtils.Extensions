using System;
using System.Collections.Generic;

namespace SangoUtils.NetOperations
{
    public class NetClientOperationHandler
    {
        private readonly Dictionary<int, BaseNetRequest> _netRequestDict = new Dictionary<int, BaseNetRequest>();
        private readonly Dictionary<int, BaseNetEvent> _netEventDict = new Dictionary<int, BaseNetEvent>();

        public void NetMessageCommandBroadcast(NetOpMessage message)
        {
            switch (message.OpCMD)
            {
                case 2:
                    {
                        NetMessageResponsedBroadcast(message);
                    }
                    break;
                case 3:
                    {
                        NetMessageEventBroadcast(message);
                    }
                    break;
            }
        }

        private void NetMessageResponsedBroadcast(NetOpMessage message)
        {
            if (_netRequestDict.TryGetValue(message.OpCode, out BaseNetRequest netRequest))
            {
                netRequest.OnOperationResponse(message.Message);
            }
            else
            {
                _netRequestDict.TryGetValue(0, out BaseNetRequest defaultNetRequest);
                defaultNetRequest?.OnOperationResponse(message.Message);
            }
        }

        private void NetMessageEventBroadcast(NetOpMessage message)
        {
            if (_netEventDict.TryGetValue(message.OpCode, out BaseNetEvent netEvent))
            {
                netEvent.OnEventData(message.Message);
            }
            else
            {
                _netEventDict.TryGetValue(0, out BaseNetEvent defaultNetEvent);
                defaultNetEvent?.OnEventData(message.Message);
            }
        }

        public void AddNetRequest(BaseNetRequest netRequest)
        {
            if (!_netRequestDict.ContainsKey(netRequest.OpCode))
            {
                _netRequestDict.Add(netRequest.OpCode, netRequest);
            }
            else
            {
            }
        }

        public T GetNetRequest<T>(int operationCode) where T : BaseNetRequest, new()
        {
            if (_netRequestDict.ContainsKey(operationCode))
            {
                return (T)_netRequestDict[operationCode];
            }
            else
            {
                T netRequest = Activator.CreateInstance<T>();
                netRequest.OnInit(operationCode, this);
                return netRequest;
            }
        }

        public void RemoveNetRequest(BaseNetRequest netRequest)
        {
            if (_netRequestDict.ContainsKey(netRequest.OpCode))
            {
                _netRequestDict.Remove(netRequest.OpCode);
            }
            else
            {
            }
        }

        public void AddNetEvent(BaseNetEvent netEvent)
        {
            if (!_netEventDict.ContainsKey(netEvent.OpCode))
            {
                _netEventDict.Add(netEvent.OpCode, netEvent);
            }
            else
            {
            }
        }

        public T GetNetEvent<T>(int operationCode) where T : BaseNetEvent, new()
        {
            if (_netEventDict.ContainsKey(operationCode))
            {
                return (T)_netEventDict[operationCode];
            }
            else
            {
                T netEvent = Activator.CreateInstance<T>();
                netEvent.OnInit(operationCode, this);
                return netEvent;
            }
        }

        public void RemoveNetEvent(BaseNetEvent netEvent)
        {
            if (_netEventDict.ContainsKey(netEvent.OpCode))
            {
                _netEventDict.Remove(netEvent.OpCode);
            }
            else
            {
            }
        }
    }
}
