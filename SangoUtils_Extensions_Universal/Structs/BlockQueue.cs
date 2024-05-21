using System;
using System.Collections.Generic;

namespace SangoUtils.Utilitys
{
    public class BlockQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly object _lock = new object();
        private readonly int _maxSize;
        private bool _isFull = false;
        public BlockQueue(int maxSize)
        {
            _maxSize = maxSize;
        }
        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
                if (_queue.Count >= _maxSize)
                {
                    _isFull = true;
                }
            }
        }
        public T Dequeue()
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty");
                }
                T item = _queue.Dequeue();
                if (_isFull)
                {
                    _isFull = false;
                }
                return item;
            }
        }
        public bool TryDequeue(out T item)
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    item = default;
                    return false;
                }
                item = _queue.Dequeue();
                if (_isFull)
                {
                    _isFull = false;
                }
                return true;
            }
        }
        public T Peek()
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    throw new InvalidOperationException("Queue is empty");
                }
                return _queue.Peek();
            }
        }
        public bool TryPeek(out T item)
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    item = default;
                    return false;
                }
                item = _queue.Peek();
                return true;
            }
        }
        public T[] ToArray()
        {
            lock (_lock)
            {
                return _queue.ToArray();
            }
        }
        public void Clear()
        {
            lock (_lock)
            {
                _queue.Clear();
                _isFull = false;
            }
        }
        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _queue.Contains(item);
            }
        }
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _queue.Count;
                }
            }
        }
        public int MaxSize
        {
            get
            {
                lock (_lock)
                {
                    return _maxSize;
                }
            }
        }
    }
}
