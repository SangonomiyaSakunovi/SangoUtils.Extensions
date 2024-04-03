using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SangoUtils.UDPClients
{
    internal abstract class UdpMessagePack
    {
        protected NetUdpEventHandler? _netUdpEventHandler;
        public int ListenProtID { get; set; }
        public UdpClient? UdpClient { get; set; }
        public IPEndPoint? _ipEndPoint;
        public IPEndPoint? IPEndPoint { get => _ipEndPoint; set => _ipEndPoint = value; }

        public Type? DataType { get; set; }

        protected string _dataReceived = "";

        public abstract void OnUdpMessage(IAsyncResult asyncResult);
    }

    internal class UdpMessagePack<T> : UdpMessagePack where T : class
    {
        public UdpMessagePack(NetUdpEventHandler handler)
        {
            _netUdpEventHandler = handler;
        }

        public override void OnUdpMessage(IAsyncResult asyncResult)
        {
            try
            {
                UdpMessagePack? udpPack = asyncResult.AsyncState as UdpMessagePack;
                if (udpPack != null && udpPack.UdpClient != null)
                {
                    byte[] dataReceivedBytes = udpPack.UdpClient.EndReceive(asyncResult, ref udpPack._ipEndPoint);
                    _dataReceived = Encoding.UTF8.GetString(dataReceivedBytes, 0, dataReceivedBytes.Length);
                    if (!string.IsNullOrEmpty(_dataReceived))
                    {
                        T? data;
                        if (typeof(T).Name == "String")
                        {
                            data = _dataReceived as T;
                        }
                        else
                        {
                            data = JsonSerializer.Deserialize<T>(_dataReceived);
                        }
                        if (data != null)
                        {
                            if (_netUdpEventHandler != null)
                            {
                                UdpData udpData = new UdpData<T>(udpPack.ListenProtID, data, typeof(T), _netUdpEventHandler);
                                _netUdpEventHandler.OnEventDataReceived(udpData);
                            }
                            else
                            {
                                throw new ArgumentNullException(nameof(udpPack));
                            }
                        }
                    }
                    udpPack.UdpClient.BeginReceive(udpPack.OnUdpMessage, udpPack);
                }
            }
            catch (Exception)
            {
                throw new NotSupportedException("A strange message received.");
            }
        }
    }
}
