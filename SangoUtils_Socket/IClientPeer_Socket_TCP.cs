using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace SangoUtils_Socket.TCP
{
    public abstract class IClientPeer_Socket_TCP
    {
        public int PeerID { get; set; }
        private SocketAsyncEventArgs _sendToClientSAEA;
        private SocketAsyncEventArgs _receiveFromClientSAEA;

        private ConcurrentQueue<byte[]> _sendMessageQueue;
        private ConcurrentQueue<byte[]> _receiveMessageQueue;

        private bool _isWaittingSendRes = false;

        private Socket _socket;

        protected abstract void OnConnected();
        protected abstract void OnMessage(string message);
        protected abstract void OnClosed();

        internal Action<int> OnClientPeerResourcesCleaned { get; set; }

        internal ConnectionStateCode _connectionState = ConnectionStateCode.None;

        internal void Init(Socket skt)
        {
            _socket = skt;
            _connectionState = ConnectionStateCode.Connected;

            _sendToClientSAEA = new SocketAsyncEventArgs();
            _sendToClientSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendToClientCompleted);

            _receiveFromClientSAEA = new SocketAsyncEventArgs();
            _receiveFromClientSAEA.SetBuffer(new byte[Socket_TCPConfig.ServerBufferCount], 0, Socket_TCPConfig.ServerBufferCount);
            _receiveFromClientSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(OnReceiveFromClientCompleted);

            _sendMessageQueue = new ConcurrentQueue<byte[]>();
            _receiveMessageQueue = new ConcurrentQueue<byte[]>();

            _socket.ReceiveAsync(_receiveFromClientSAEA);

            OnConnected();
        }

        public void Send(string message)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(message);
            ProcessingSend(bytes);
        }

        private void ProcessingSend(byte[] bytes)
        {
            if (_socket != null)
            {
                if (_isWaittingSendRes)
                {
                    _sendMessageQueue.Enqueue(bytes);
                }
                else
                {
                    _sendToClientSAEA.SetBuffer(bytes, 0, bytes.Length);
                    _isWaittingSendRes = true;
                    _socket.SendAsync(_sendToClientSAEA);
                }
            }
        }

        private void OnSendToClientCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError != SocketError.Success) return;

            Socket socket = sender as Socket;
            if (socket != null)
            {
                string message = Encoding.Default.GetString(socketAsyncEventArgs.Buffer);
                SocketLogger.Info("Client : Send message" + message + "to Client" + socket.RemoteEndPoint.ToString());
            }

            _isWaittingSendRes = false;
            if (_sendMessageQueue.Count > 0)
            {
                if (_sendMessageQueue.TryDequeue(out byte[] message))
                {
                    ProcessingSend(message);
                }
            }
        }

        private void OnReceiveFromClientCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (socketAsyncEventArgs.SocketError == SocketError.OperationAborted) { SocketLogger.Warning("Abortion!!!!"); return; }
            Socket socket = sender as Socket;
            if (socketAsyncEventArgs.SocketError == SocketError.Success && socketAsyncEventArgs.BytesTransferred > 0)
            {
                //string ipAddress = socketAsyncEventArgs.RemoteEndPoint.ToString();
                byte[] bytes = new byte[socketAsyncEventArgs.BytesTransferred];
                if (socketAsyncEventArgs.Buffer != null)
                {
                    Buffer.BlockCopy(socketAsyncEventArgs.Buffer, 0, bytes, 0, socketAsyncEventArgs.BytesTransferred);
                    _receiveMessageQueue.Enqueue(bytes);
                }

                if (socket != null)
                {
                    socket.ReceiveAsync(socketAsyncEventArgs);
                }

                ProcessingReceive();
            }
            else if (socketAsyncEventArgs.BytesTransferred == 0)
            {
                if (socketAsyncEventArgs.SocketError == SocketError.Success)
                {
                    SocketLogger.Warning("Client Close the connection.");
                }
                else
                {
                    SocketLogger.Warning("Server Close the connection.");
                    DisConnect();
                }
                CleanResources();
            }
            else
            {
                SocketLogger.Error("Something Wrong Happened.");
                DisConnect();
                CleanResources();
            }
        }

        private void ProcessingReceive()
        {
            if (_receiveMessageQueue.Count > 0)
            {
                if (_receiveMessageQueue.TryDequeue(out byte[] bytes))
                {
                    string message = System.Text.Encoding.Default.GetString(bytes);
                    OnMessage(message);
                }
            }
            ProcessingReceive();
        }

        internal void DisConnect()
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

        internal void CleanResources()
        {
            _connectionState = ConnectionStateCode.Disconnected;
            if (_sendToClientSAEA != null)
            {
                _sendToClientSAEA.Completed -= OnSendToClientCompleted;
                _sendToClientSAEA.Dispose();
                _sendToClientSAEA = null;
            }
            if (_receiveFromClientSAEA != null)
            {
                _receiveFromClientSAEA.Completed -= OnReceiveFromClientCompleted;
                _receiveFromClientSAEA.Dispose();
                _receiveFromClientSAEA = null;
            }
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }

            OnClientPeerResourcesCleaned?.Invoke(PeerID);

            SocketLogger.Start($"Socket_TCP_Peer: [ {PeerID} ] is Offline, bye.");
            OnClosed();
        }
    }

    internal enum ConnectionStateCode
    {
        None,
        Disconnected,
        Connected
    }
}
