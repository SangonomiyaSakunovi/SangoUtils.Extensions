using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SangoUtils_UDP
{
    public abstract class UdpClientPeer
    {
        public int ListenerPortId { get; set; }
        protected Type? _dataType;
        protected Thread? _receiveUdpMessageThread;

        protected abstract void ThreadReceive<T>() where T : class;
    }

    public class UdpClientPeer<T> : UdpClientPeer where T : class
    {
        private NetUdpEventHandler _netUdpEventHandler;

        public UdpClientPeer(int udpListenerPort, NetUdpEventHandler handler)
        {
            ListenerPortId = udpListenerPort;
            _dataType = typeof(T);
            _netUdpEventHandler = handler;
        }

        public void Open()
        {
            ThreadReceive<T>();
        }

        protected override void ThreadReceive<K>() where K : class
        {
            _receiveUdpMessageThread = new Thread(() =>
            {
                IPEndPoint udpListenerIpEndPoint = new IPEndPoint(IPAddress.Any, ListenerPortId);
                UdpClient udpListenerClient = new UdpClient(udpListenerIpEndPoint);
                UdpMessagePack<K> udpPack = new UdpMessagePack<K>(_netUdpEventHandler);
                udpPack.ListenProtID = ListenerPortId;
                udpPack.IPEndPoint = udpListenerIpEndPoint;
                udpPack.UdpClient = udpListenerClient;
                udpPack.DataType = _dataType;
                udpPack.UdpClient.BeginReceive(udpPack.OnUdpMessage, udpPack);
            })
            {
                IsBackground = true
            };
            _receiveUdpMessageThread.Start();
        }
    }
}
