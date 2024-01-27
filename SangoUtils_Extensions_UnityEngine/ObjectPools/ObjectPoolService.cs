using SangoUtils_Extensions_UnityEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Pool;

namespace SangoUtils_Extensions_UnityEngine.ObjectPools
{
    public class ObjectPoolService : UnitySingleton<ObjectPoolService>
    {
        //private Dictionary<Type, ObjectPool> _objectPoolDict = new Dictionary<Type, ObjectPool>();

        //private ObjectPool<List<GameObject>> _childrenObjectsPool;

        //public void OnInit()
        //{
        //    _childrenObjectsPool = new ObjectPool<List<GameObject>>(() => new List<GameObject>());
        //}

        //public List<GameObject> GetGameObjects()
        //{
        //    return _childrenObjectsPool.Get();
        //}

        //public void Release(List<GameObject> gameObjects)
        //{
        //    _childrenObjectsPool.Release(gameObjects);
        //}

        //public void OnUpdate()
        //{

        //}

        //public void OnDispose()
        //{
        //    foreach (var item in _objectPoolDict)
        //    {
        //        item.Value.Dispose();
        //    }
        //    _objectPoolDict.Clear();
        //}

        //public void CreateObjectPool<T>(int initCount, int maxCount) where T : class
        //{
        //    Type type = typeof(T);
        //    if (!_objectPoolDict.ContainsKey(type))
        //    {
        //        ObjectPool objectPool = new ObjectPool();
        //        objectPool.Init(initCount, maxCount);
        //        _objectPoolDict.Add(type, objectPool);
        //    }
        //}

        //public T GetObject<T>() where T : class
        //{
        //    Type type = typeof(T);
        //    if (_objectPoolDict.TryGetValue(type, out UnityEngine.Pool.ObjectPool objectPool))
        //    {
        //        return objectPool.GetObject<T>();
        //    }
        //    return null;
        //}

        //public void RecycleObject<T>(T obj) where T : class
        //{
        //    Type type = typeof(T);
        //    if (_objectPoolDict.TryGetValue(type, out ObjectPool objectPool))
        //    {
        //        objectPool.RecycleObject(obj);
        //    }
        //}

        //public void RecycleAllObject<T>() where T : class
        //{
        //    Type type = typeof(T);
        //    if (_objectPoolDict.TryGetValue(type, out ObjectPool objectPool))
        //    {
        //        objectPool.RecycleAllObject();
        //    }
        //}   
    }
}
