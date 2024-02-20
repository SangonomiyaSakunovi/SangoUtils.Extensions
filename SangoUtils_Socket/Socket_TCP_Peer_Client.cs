using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SangoUtils_Socket.TCP
{
    public class Socket_TCP_Peer_Client
    {
        public Action<string>? OnMessage;

        private Socket? _socket;

        private string _ipAddressServer;
        private int _portServer;

        private IPEndPoint _serverEndPoint;
        private SocketAsyncEventArgs _connetToServerSAEA;
        private SocketAsyncEventArgs _sendToServerSAEA;
        private SocketAsyncEventArgs _receiveFromServerSAEA;

        private bool _isNeedReconnect = false;

        public void OpenAsConsoleClient(string ip, int port)
        {
            SocketLogger.SetLogger(SocketRunnerType.ConsoleProject);
            _ipAddressServer = ip;
            _portServer = port;
            OpenAsClient(ip, port);
        }

        public void OpenAsUnityClient(string ip, int port)
        {
            SocketLogger.SetLogger(SocketRunnerType.UnityProject);
            _ipAddressServer = ip;
            _portServer = port;
            OpenAsClient(ip, port);
        }

        public void OnUpdate()
        {
            if (_isNeedReconnect)
            {                
                OpenAsClient(_ipAddressServer, _portServer);
                _isNeedReconnect = false;
            }
        }

        public void Send(string message)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(message);

            _sendToServerSAEA.SetBuffer(bytes, 0, bytes.Length);
            if (_socket != null)
            {
                _socket.SendAsync(_sendToServerSAEA);
            }
        }

        public void CloseAsClient()
        {
            DisConnect();
            CleanResources();
        }

        private void OpenAsClient(string ip, int port)
        {
            SocketLogger.Start("Socket_TCP_Peer Init as Client, hello to the world.");
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _connetToServerSAEA = new SocketAsyncEventArgs { RemoteEndPoint = _serverEndPoint };
            _sendToServerSAEA = new SocketAsyncEventArgs();
            _receiveFromServerSAEA = new SocketAsyncEventArgs();

            _connetToServerSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectedServerCompleted);
            _sendToServerSAEA.Completed += OnSendToServerCompleted;

            _receiveFromServerSAEA.SetBuffer(new byte[Socket_TCPConfig.ServerBufferCount], 0, Socket_TCPConfig.ServerBufferCount);
            _receiveFromServerSAEA.Completed += OnReceiveFromServerCompleted;
            _receiveFromServerSAEA.RemoteEndPoint = _serverEndPoint;

            _socket.ConnectAsync(_connetToServerSAEA);
        }

        private void OnConnectedServerCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError != SocketError.Success)
            {
                _isNeedReconnect = true;
                return;
            }

            Socket? socket = sender as Socket;
            if (socket != null)
            {
                string ipAddressServer = socket.RemoteEndPoint.ToString();
                SocketLogger.Info("Connected to Server: {0}", ipAddressServer);

                socket.ReceiveAsync(_receiveFromServerSAEA);
            }
        }

        private void OnSendToServerCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError != SocketError.Success) return;

            Socket? socket = sender as Socket;
            if (socket != null)
            {
                string message = Encoding.Default.GetString(socketAsyncEventArgs.Buffer);
                SocketLogger.Info("Client : Send message" + message + "to Serer" + socket.RemoteEndPoint.ToString());
            }
        }

        private void OnReceiveFromServerCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError == SocketError.OperationAborted) return;

            if (socketAsyncEventArgs.SocketError == SocketError.Success && socketAsyncEventArgs.BytesTransferred > 0)
            {
                //string ipAddress = socketAsyncEventArgs.RemoteEndPoint.ToString();
                byte[] bytes = new byte[socketAsyncEventArgs.BytesTransferred];
                if (socketAsyncEventArgs.Buffer != null)
                {
                    Buffer.BlockCopy(socketAsyncEventArgs.Buffer, 0, bytes, 0, socketAsyncEventArgs.BytesTransferred);
                    string message = System.Text.Encoding.Default.GetString(bytes);
                    OnMessage?.Invoke(message);
                }
                Socket? socket = sender as Socket;
                if (socket != null)
                {
                    socket.ReceiveAsync(socketAsyncEventArgs);
                }
            }
            else if (socketAsyncEventArgs.BytesTransferred == 0)
            {
                if (socketAsyncEventArgs.SocketError == SocketError.Success)
                {
                    SocketLogger.Warning("Client Close the connection.");
                    DisConnect();
                }
                else
                {
                    SocketLogger.Warning("Server Close the connection.");
                }
                CleanResources();
            }

        }

        private void DisConnect()
        {            
            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
                catch (SocketException)
                {

                }
                finally
                {
                    _socket.Close();
                    _socket = null;
                }
            }
        }

        private void CleanResources()
        {
            if (_sendToServerSAEA!=null)
            {
                _sendToServerSAEA.Completed -= OnSendToServerCompleted;
                _sendToServerSAEA.Dispose();
                _sendToServerSAEA = null;
            }            
            if (_receiveFromServerSAEA != null)
            {
                _receiveFromServerSAEA.Completed -= OnReceiveFromServerCompleted;
                _receiveFromServerSAEA.Dispose();
                _receiveFromServerSAEA = null;
            }
            if (_connetToServerSAEA != null)
            {
                _connetToServerSAEA.Completed -= OnConnectedServerCompleted;
                _connetToServerSAEA.Dispose();
                _connetToServerSAEA = null;
            }
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }

            SocketLogger.Start("Socket_TCP_Peer is Offline, bye.");
        }
    }
}
