using SangoUtils.Sockets.TCP;
using System.Collections.Generic;

namespace SangoUtils.Sockets
{
    public class Socket_TCP_ClientPeerPool<T> where T : IClientPeer_Socket_TCP, new()
    {
        private readonly Stack<T> _clientPeerStack;
        public int Size => _clientPeerStack.Count;

        public Socket_TCP_ClientPeerPool(int capacity)
        {
            _clientPeerStack = new Stack<T>(capacity);
        }

        public T Pop()
        {
            lock (_clientPeerStack)
            {
                return _clientPeerStack.Pop();
            }
        }

        public void Push(T peer)
        {
            if (peer == null)
            {
                SocketsLogger.Error("The clientPeer to pool can`t be null");
                return;
            }
            lock (_clientPeerStack)
            {
                _clientPeerStack.Push(peer);
            }
        }
    }
}
