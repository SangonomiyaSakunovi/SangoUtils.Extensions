using SangoNetProtol;
using SangoUtils_NetOperation;
using System;
using System.Collections.Generic;

public class NetClientOperationHandler
{
    private readonly Dictionary<NetOperationCode, BaseNetRequest> _netRequestDict = new Dictionary<NetOperationCode, BaseNetRequest>();
    private readonly Dictionary<NetOperationCode, BaseNetEvent> _netEventDict = new Dictionary<NetOperationCode, BaseNetEvent>();
    private readonly Dictionary<NetOperationCode, BaseNetBroadcast> _netBroadcastDict = new Dictionary<NetOperationCode, BaseNetBroadcast>();

    public void NetMessageCommandBroadcast(SangoNetMessage sangoNetMessage)
    {
        switch (sangoNetMessage.NetMessageHead.NetMessageCommandCode)
        {
            case NetMessageCommandCode.NetOperationResponse:
                {
                    NetMessageResponsedBroadcast(sangoNetMessage);
                }
                break;
            case NetMessageCommandCode.NetEventData:
                {
                    NetMessageEventBroadcast(sangoNetMessage);
                }
                break;
            case NetMessageCommandCode.NetBroadcast:
                {
                    NetMessageBroadcastBroadcast(sangoNetMessage);
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
            _netRequestDict.TryGetValue(NetOperationCode.Default, out BaseNetRequest defaultNetRequest);
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
            _netEventDict.TryGetValue(NetOperationCode.Default, out BaseNetEvent defaultNetEvent);
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
            _netBroadcastDict.TryGetValue(NetOperationCode.Default, out BaseNetBroadcast defaultNetBroadcast);
            defaultNetBroadcast?.OnBroadcast(sangoNetMessage.NetMessageBody.NetMessageStr);
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

    public T GetNetRequest<T>(NetOperationCode operationCode) where T : BaseNetRequest, new()
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

    public T GetNetEvent<T>(NetOperationCode operationCode) where T : BaseNetEvent, new()
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

    public T GetNetBroadcast<T>(NetOperationCode operationCode) where T : BaseNetBroadcast, new()
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
}
