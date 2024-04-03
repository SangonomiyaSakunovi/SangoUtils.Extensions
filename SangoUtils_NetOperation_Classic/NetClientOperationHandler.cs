using SangoNetProtocol_Classic;
using System;
using System.Collections.Generic;

namespace SangoUtils.NetOperationClassic
{
    public class NetClientOperationHandler
    {
        private readonly Dictionary<int, BaseNetRequest> _netRequestDict = new Dictionary<int, BaseNetRequest>();
        private readonly Dictionary<int, BaseNetEvent> _netEventDict = new Dictionary<int, BaseNetEvent>();
        private readonly Dictionary<int, BaseNetBroadcast> _netBroadcastDict = new Dictionary<int, BaseNetBroadcast>();
        private readonly Dictionary<int, BaseNetUdpMessage> _netUdpMessagesDict = new Dictionary<int, BaseNetUdpMessage>();

        public void NetMessageCommandBroadcast(SangoNetMessage sangoNetMessage)
        {
            switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
            {
                case 2:
                    {
                        NetMessageResponsedBroadcast(sangoNetMessage);
                    }
                    break;
                case 4:
                    {
                        NetMessageEventBroadcast(sangoNetMessage);
                    }
                    break;
                case 5:
                    {
                        NetMessageBroadcastBroadcast(sangoNetMessage);
                    }
                    break;
                case 6:
                    {
                        NetUdpMessageBroadcast(sangoNetMessage);
                    }
                    break;
            }
        }

        private void NetMessageResponsedBroadcast(SangoNetMessage sangoNetMessage)
        {
            if (_netRequestDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetRequest netRequest))
            {
                netRequest.OnOperationResponse(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
            else
            {
                _netRequestDict.TryGetValue(1, out BaseNetRequest defaultNetRequest);
                defaultNetRequest?.OnOperationResponse(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
        }

        private void NetMessageEventBroadcast(SangoNetMessage sangoNetMessage)
        {
            if (_netEventDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetEvent netEvent))
            {
                netEvent.OnEventData(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
            else
            {
                _netEventDict.TryGetValue(1, out BaseNetEvent defaultNetEvent);
                defaultNetEvent?.OnEventData(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
        }

        private void NetMessageBroadcastBroadcast(SangoNetMessage sangoNetMessage)
        {
            if (_netBroadcastDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetBroadcast netBroadcast))
            {
                netBroadcast.OnBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
            else
            {
                _netBroadcastDict.TryGetValue(1, out BaseNetBroadcast defaultNetBroadcast);
                defaultNetBroadcast?.OnBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
        }

        private void NetUdpMessageBroadcast(SangoNetMessage sangoNetMessage)
        {
            if (_netUdpMessagesDict.TryGetValue(sangoNetMessage.NetMessageHead.NetOperationCode, out BaseNetUdpMessage netUdpMessage))
            {
                netUdpMessage.OnUdpMessage(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
            else
            {
                _netUdpMessagesDict.TryGetValue(1, out BaseNetUdpMessage defaultNetUdpMessage);
                defaultNetUdpMessage?.OnUdpMessage(sangoNetMessage.NetMessageBody.NetMessageStr);
            }
        }

        public void AddNetRequest(BaseNetRequest netRequest)
        {
            if (!_netRequestDict.ContainsKey(netRequest.NetOperationCode))
            {
                _netRequestDict.Add(netRequest.NetOperationCode, netRequest);
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
            if (_netRequestDict.ContainsKey(netRequest.NetOperationCode))
            {
                _netRequestDict.Remove(netRequest.NetOperationCode);
            }
            else
            {
            }
        }

        public void AddNetEvent(BaseNetEvent netEvent)
        {
            if (!_netEventDict.ContainsKey(netEvent.NetOperationCode))
            {
                _netEventDict.Add(netEvent.NetOperationCode, netEvent);
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
            if (_netEventDict.ContainsKey(netEvent.NetOperationCode))
            {
                _netEventDict.Remove(netEvent.NetOperationCode);
            }
            else
            {
            }
        }

        public void AddNetBroadcast(BaseNetBroadcast netBroadcast)
        {
            if (!_netBroadcastDict.ContainsKey(netBroadcast.NetOperationCode))
            {
                _netBroadcastDict.Add(netBroadcast.NetOperationCode, netBroadcast);
            }
            else
            {
            }
        }

        public T GetNetBroadcast<T>(int operationCode) where T : BaseNetBroadcast, new()
        {
            if (_netBroadcastDict.ContainsKey(operationCode))
            {
                return (T)_netBroadcastDict[operationCode];
            }
            else
            {
                T netBroadcast = Activator.CreateInstance<T>();
                netBroadcast.OnInit(operationCode, this);
                return netBroadcast;
            }
        }

        public void RemoveNetBroadcast(BaseNetBroadcast netBroadcast)
        {
            if (_netBroadcastDict.ContainsKey(netBroadcast.NetOperationCode))
            {
                _netBroadcastDict.Remove(netBroadcast.NetOperationCode);
            }
            else
            {
            }
        }

        public void AddNetUdpMessage(BaseNetUdpMessage netUdpMessage)
        {
            if (!_netUdpMessagesDict.ContainsKey(netUdpMessage.NetOperationCode))
            {
                _netUdpMessagesDict.Add(netUdpMessage.NetOperationCode, netUdpMessage);
            }
            else
            {
            }
        }

        public T GetNetUdpMessage<T>(int operationCode) where T : BaseNetUdpMessage, new()
        {
            if (_netUdpMessagesDict.ContainsKey(operationCode))
            {
                return (T)_netUdpMessagesDict[operationCode];
            }
            else
            {
                T netUdpMessage = Activator.CreateInstance<T>();
                netUdpMessage.OnInit(operationCode, this);
                return netUdpMessage;
            }
        }

        public void RemoveNetUdpMessage(BaseNetUdpMessage netUdpMessage)
        {
            if (_netUdpMessagesDict.ContainsKey(netUdpMessage.NetOperationCode))
            {
                _netUdpMessagesDict.Remove(netUdpMessage.NetOperationCode);
            }
            else
            {
            }
        }
    }
}
