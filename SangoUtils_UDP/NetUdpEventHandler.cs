using System;
using System.Collections.Generic;

namespace SangoUtils_UDP
{
    public class NetUdpEventHandler
    {
        private readonly Dictionary<int, UdpClientPeer> _udpClientPeerDict = new Dictionary<int, UdpClientPeer>();
        private readonly Dictionary<int, BaseUdpEvent> _udpEventDict = new Dictionary<int, BaseUdpEvent>();

        private readonly List<UdpData> _udpDatas = new List<UdpData>(10);

        public void AddUdpClientPeer(UdpClientPeer peer)
        {
            _udpClientPeerDict.Add(peer.ListenerPortId, peer);
        }

        public void OnUpdate()
        {
            HandleEventReceivedData();
        }

        private void HandleEventReceivedData()
        {
            if (_udpDatas.Count == 0) return;
            for (int i = 0; i < _udpDatas.Count; i++)
            {
                UdpData data = _udpDatas[i];
                data.OnDataSync();
            }
            _udpDatas.Clear();
        }

        public void OnEventDataReceived(UdpData udpData)
        {
            lock ("locker_AddUpdEventReceivedData")
            {
                _udpDatas.Add(udpData);
            }
        }

        public void UdpEventBroadcast<T>(T data, int eventId) where T : class
        {
            if (_udpEventDict.TryGetValue(eventId, out BaseUdpEvent netEvent))
            {
                netEvent.OnEventDataReceived<T>(data);
            }
        }

        public void AddUdpEvent(BaseUdpEvent evt)
        {
            _udpEventDict.Add(evt.UdpEventPortId, evt);
        }

        public T GetUdpEvent<T>(int udpEventPortId) where T : BaseUdpEvent, new()
        {
            if (_udpEventDict.ContainsKey(udpEventPortId))
            {
                return (T)_udpEventDict[udpEventPortId];
            }
            else
            {
                T udpEvent = Activator.CreateInstance<T>();
                udpEvent.OnInit(udpEventPortId, this);
                return udpEvent;
            }
        }

        public void RemoveUdpEvent(BaseUdpEvent evt)
        {
            _udpEventDict.Remove(evt.UdpEventPortId);
        }
    }
}
