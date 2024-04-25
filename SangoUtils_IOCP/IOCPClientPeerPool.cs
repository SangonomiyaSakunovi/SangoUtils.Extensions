using System.Collections.Generic;

namespace SangoUtils.IOCPs
{
    public class IOCPClientPeerPool<T> where T : IClientPeer_IOCP, new()
    {
        private readonly Stack<T> _clientPeerStack;
        public int Size => _clientPeerStack.Count;

        public IOCPClientPeerPool(int capacity)
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
                IOCPLogger.Error("The clientPeer to pool can`t be null");
                return;
            }
            lock (_clientPeerStack)
            {
                _clientPeerStack.Push(peer);
            }
        }
    }
}