using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace SangoUtils.Behaviours_Unity.PoolsOPs
{
    internal class ObjectPoolSample
    {
        private Dictionary<Type, ObjectPool<GameObject>> _objectPoolDict = new Dictionary<Type, ObjectPool<GameObject>>();

        private ObjectPool<List<GameObject>> _childrenObjectsPool;

        public void OnInit()
        {
            _childrenObjectsPool = new ObjectPool<List<GameObject>>(() => new List<GameObject>());
        }

        public List<GameObject> GetGameObjects()
        {
            return _childrenObjectsPool.Get();
        }

        public void Release(List<GameObject> gameObjects)
        {
            _childrenObjectsPool.Release(gameObjects);
        }

        public void OnUpdate()
        {

        }

        public void OnDispose()
        {
            foreach (var item in _objectPoolDict)
            {
                item.Value.Dispose();
            }
            _objectPoolDict.Clear();
        }

        public void CreateObjectPool<T>(int initCount, int maxCount) where T : class
        {
            Type type = typeof(T);
            if (!_objectPoolDict.ContainsKey(type))
            {
                ObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(null);
                _objectPoolDict.Add(type, objectPool);
            }
        }

        public GameObject GetObject<T>() where T : class
        {
            Type type = typeof(T);
            if (_objectPoolDict.TryGetValue(type, out ObjectPool<GameObject> objectPool))
            {
                return objectPool.Get();
            }
            return null;
        }
    }
}
