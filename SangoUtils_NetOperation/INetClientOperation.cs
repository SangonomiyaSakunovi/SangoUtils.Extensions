﻿using SangoNetProtol;

namespace SangoUtils.NetOperations
{
    public interface INetClientOperation
    {
        public void OpenClient();

        public void CloseClient();

        public void SendOperationRequest(NetOperationCode operationCode, string messageStr);

        public void SendOperationBroadcast(NetOperationCode operationCode, string messageStr);

        public void OnMessageReceived(SangoNetMessage sangoNetMessage);

        public T GetNetRequest<T>(NetOperationCode netOperationCode) where T : BaseNetRequest, new();

        public T GetNetEvent<T>(NetOperationCode operationCode) where T : BaseNetEvent, new();

        public T GetNetBroadcast<T>(NetOperationCode operationCode) where T : BaseNetBroadcast, new();
    }
}
