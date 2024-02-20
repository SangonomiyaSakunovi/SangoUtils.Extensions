using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace SangoUtils_IOCP
{
    public abstract class IClientPeer_IOCP
    {
        public int PeerId { get; set; }
        private readonly SocketAsyncEventArgs _receiveAsyncEventArgs = new SocketAsyncEventArgs();
        private readonly SocketAsyncEventArgs _sendAsyncEventArgs = new SocketAsyncEventArgs();

        private Socket? _socket;
        private List<byte> _readList = new List<byte>();
        private Queue<byte[]> _cacheQueue = new Queue<byte[]>();
        private bool _isWrite = false;

        public Action<int>? OnClientPeerClosed { get; set; }
        internal ConnectionStateCode _connectionState = ConnectionStateCode.None;

        public IClientPeer_IOCP()
        {
            _receiveAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
            _sendAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnIO_Completed);
            _receiveAsyncEventArgs.SetBuffer(new byte[IOCPConfig.ServerBufferCount], 0, IOCPConfig.ServerBufferCount);
        }

        protected abstract void OnOpen();

        protected abstract void OnClosed();

        protected abstract void OnBinary(byte[] byteMessages);

        internal void Init(Socket skt)
        {
            IOCPLogger.Info("Init Client Peer, starting Recieve Async.");
            _socket = skt;
            _connectionState = ConnectionStateCode.Connected;
            OnOpen();
            AsyncReceive();
        }

        private void AsyncReceive()
        {
            if (_socket != null)
            {
                bool isReceiveWaiting = _socket.ReceiveAsync(_receiveAsyncEventArgs);
                if (isReceiveWaiting == false)
                {
                    ProcessReceive();
                }
            }
        }

        private void ProcessReceive()
        {
            if (_receiveAsyncEventArgs.SocketError == SocketError.Success && _receiveAsyncEventArgs.BytesTransferred > 0)
            {
                byte[] bytes = new byte[_receiveAsyncEventArgs.BytesTransferred];
                if (_receiveAsyncEventArgs.Buffer != null)
                {
                    Buffer.BlockCopy(_receiveAsyncEventArgs.Buffer, 0, bytes, 0, _receiveAsyncEventArgs.BytesTransferred);
                    _readList.AddRange(bytes);
                }
                ProcessByteList();
                AsyncReceive();
            }
            else
            {
                IOCPLogger.Warning("IClientPeer:{0}  Close:{1}", PeerId, _receiveAsyncEventArgs.SocketError.ToString());
                OnClientClosed();
            }
        }

        private void ProcessByteList()
        {
            byte[]? byteMessages = IOCPUtils.SplitLogicBytes(ref _readList);
            if (byteMessages != null)
            {
                OnBinary(byteMessages);
                ProcessByteList();
            }
        }

        public bool Send(byte[] byteMessage)
        {
            byte[] bytes = IOCPUtils.PackMessageLengthInfo(byteMessage);
            return SendPacked(bytes);
        }

        public byte[] GetPackMessage(byte[] byteMessage)
        {
            return IOCPUtils.PackMessageLengthInfo(byteMessage);
        }

        public bool SendPacked(byte[] bytePackMessages)
        {
            if (_socket == null)
            {
                IOCPLogger.Error("Socket Error: Socket is null.");
                return false;
            }
            if (_connectionState != ConnectionStateCode.Connected)
            {
                IOCPLogger.Warning("Connection is break, can`t send net message.");
                return false;
            }
            if (_isWrite)
            {
                _cacheQueue.Enqueue(bytePackMessages);
                return true;
            }
            _isWrite = true;
            _sendAsyncEventArgs.SetBuffer(bytePackMessages, 0, bytePackMessages.Length);
            bool isSendWaiting = _socket.SendAsync(_sendAsyncEventArgs);
            if (isSendWaiting == false)
            {
                ProcessSend();
            }
            return true;
        }

        private void ProcessSend()
        {
            if (_sendAsyncEventArgs.SocketError == SocketError.Success)
            {
                _isWrite = false;
                if (_cacheQueue.Count > 0)
                {
                    byte[] item = _cacheQueue.Dequeue();
                    SendPacked(item);
                }
            }
            else
            {
                IOCPLogger.Error("Process Send Error: {0}", _sendAsyncEventArgs.SocketError.ToString());
                OnClientClosed();
            }
        }

        internal void OnClientClosed()
        {
            if (_socket != null)
            {
                _connectionState = ConnectionStateCode.Disconnected;
                OnClosed();
                if (OnClientPeerClosed != null)
                {
                    OnClientPeerClosed(PeerId);
                }
                _readList.Clear();
                _cacheQueue.Clear();
                _isWrite = false;
                try
                {
                    _socket.Shutdown(SocketShutdown.Send);
                }
                catch (Exception e)
                {
                    IOCPLogger.Error("Shutdown socket Error:{0}", e.ToString());
                }
                finally
                {
                    _socket.Close();
                    _socket = null;
                    IOCPLogger.Done("Client is Offline");
                }
            }
        }

        private void OnIO_Completed(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            switch (socketAsyncEventArgs.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive();
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend();
                    break;
                default:
                    IOCPLogger.Warning("The last operation completed on the socket was not a receive or send.");
                    break;
            }
        }
    }

    internal enum ConnectionStateCode
    {
        None,
        Disconnected,
        Connected
    }
}

