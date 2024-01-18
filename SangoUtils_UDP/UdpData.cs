using System;

namespace SangoUtils_UDP
{
    public abstract class UdpData
    {
        protected int _listenProtID;
        protected Type? _dataType;
        protected NetUdpEventHandler? _netUdpEventHandler;

        public abstract void OnDataSync();
    }

    public class UdpData<T> : UdpData where T : class
    {
        private readonly T _dataReceived;

        public UdpData(int listenProtId, T dataReceived, Type dataType, NetUdpEventHandler handler)
        {
            _listenProtID = listenProtId;
            _dataReceived = dataReceived;
            _dataType = dataType;
            _netUdpEventHandler = handler;
        }

        public override void OnDataSync()
        {
            if (_netUdpEventHandler != null)
            {
                _netUdpEventHandler.UdpEventBroadcast<T>(_dataReceived, _listenProtID);
            }
            else
            {
                throw new ArgumentNullException(nameof(_netUdpEventHandler));
            }
        }
    }
}
