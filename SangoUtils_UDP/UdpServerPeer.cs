using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SangoUtils_UDP
{
    public abstract class UdpServerPeer
    {
        public int SendPortId { get; set; }
        protected Type? _dataType;

        public abstract void Send<T>(T message) where T : class;
    }

    public class UdpServerPeer<T> : UdpServerPeer where T : class
    {
        private IPAddress _ipAddress;
        private int _port;

        private UdpClient _udpServer;
        private IPEndPoint _remotePoint;

        public UdpServerPeer(IPAddress udpSendIPAddress, int udpSendPort)
        {
            _ipAddress = udpSendIPAddress;
            _port = udpSendPort;
                                    
            SendPortId = udpSendPort;
            _dataType = typeof(T);

            _udpServer = new UdpClient();
            _remotePoint = new IPEndPoint(_ipAddress, _port);
            _udpServer.Connect(_remotePoint);
        }

        public override void Send<K>(K message) where K : class
        {
            
            if (typeof(T).Name == "String")
            {
                string? data = message as string;
                if (data != null)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    _udpServer.Send(bytes, bytes.Length);
                }
            }
        }

        public void Close()
        {
            _udpServer.Close();
        }
    }
}
