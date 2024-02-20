using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SangoUtils_Socket.TCP
{
    public class Socket_TCP_Peer_Server<T> where T : IClientPeer_Socket_TCP, new()
    {
        private int _currentConnectCount = 0;
        private int _backLog = Socket_TCPConfig.ServerBackLogCount;
        private int _maxConnectCount = Socket_TCPConfig.ServerMaxConnectCount;

        private Semaphore? _acceptSeamaphore;
        private Socket_TCP_ClientPeerPool<T>? _peerPool;
        private ConcurrentDictionary<int, T> _peerDict;

        private Socket? _socket;
        private IPEndPoint _serverEndPoint;

        private SocketAsyncEventArgs _connectToClientSAEA;

        public void OpenAsServer(string ip, int port, int maxConnectCount)
        {
            SocketLogger.SetLogger(SocketRunnerType.ConsoleProject);
            SocketLogger.Start("Socket_TCP ClientPeer Init as Server, hello to the world.");
            _currentConnectCount = 0;
            _acceptSeamaphore = new Semaphore(maxConnectCount, maxConnectCount);
            _peerPool = new Socket_TCP_ClientPeerPool<T>(maxConnectCount);
            for (int i = 0; i < maxConnectCount; i++)
            {
                T peer = new T
                {
                    PeerID = i,
                };
                _peerPool.Push(peer);
            }
            _peerDict = new ConcurrentDictionary<int, T>();
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(_serverEndPoint);
            _socket.Listen(_backLog);

            _connectToClientSAEA = new SocketAsyncEventArgs();
            _connectToClientSAEA.Completed += OnAcceptCompleted;

            AsyncAccept();
        }

        public void ClosedAsServer()
        {
            DisConnect();
        }

        public List<T> GetAllClientPeerList()
        {
            return _peerDict.Values.ToList();
        }

        private void AsyncAccept()
        {
            if (_acceptSeamaphore != null && _socket != null)
            {
                _connectToClientSAEA.AcceptSocket = null;
                _acceptSeamaphore.WaitOne();
                _socket.AcceptAsync(_connectToClientSAEA);
            }
            else
            {
                SocketLogger.Error("IClientPeer Error: socket or seamaphore is null.");
            }
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs socketAsyncEventArgs)
        {
            if (_peerPool != null && _peerDict != null)
            {
                Socket peerSocket = socketAsyncEventArgs.AcceptSocket;
                if (peerSocket != null)
                {
                    Interlocked.Increment(ref _currentConnectCount);
                    T peer = _peerPool.Pop();
                    peer.Init(peerSocket);
                    peer.OnClientPeerResourcesCleaned = OnClientPeerResourcesCleaned;

                    _peerDict.TryAdd(peer.PeerID, peer);
                }
                AsyncAccept();
            }
            else
            {
                SocketLogger.Error("IClientPeer Error: peerDict or peerPool is null.");
            }
        }

        private void OnClientPeerResourcesCleaned(int peerId)
        {
            if (_peerDict != null && _peerPool != null && _acceptSeamaphore != null)
            {
                if (_peerDict.TryRemove(peerId, out T peer))
                {
                    _peerPool.Push(peer);
                    Interlocked.Decrement(ref _currentConnectCount);
                    _acceptSeamaphore.Release();
                }
                else
                {
                    SocketLogger.Error($"IClientPeer: [ {peerId} ] can`t find in server peerList.");
                }
            }
            else
            {
                SocketLogger.Error("IClientPeer Error: peerDict or peerPool or seamaphore is null.");
            }
        }

        private void DisConnect()
        {
            if (_peerDict != null)
            {
                foreach (var item in _peerDict.Values)
                {
                    item.DisConnect();
                    item.CleanResources();
                }
                _peerDict.Clear();
                _peerDict = null;
            }
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;
            }
        }
    }
}
