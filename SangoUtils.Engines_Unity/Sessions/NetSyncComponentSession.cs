using SangoUtils.Bases_Unity;
using SangoUtils.Bases_Unity.NetSyncs;
using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SangoUtils.Engines_Unity
{
    public class NetSyncComponentSession : MonoBehaviour
    {
#pragma warning disable CS8618
        public static NetSyncComponentSession Instance { get; private set; }
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        private readonly ConcurrentDictionary<int, NetSyncSubspaceObjectPack> _syncSubspaceObjectDict_Locals = new ConcurrentDictionary<int, NetSyncSubspaceObjectPack>();

        /// <summary>
        /// Warning: You must call this API before using any other APIs.
        /// </summary>
        public void Initialize()
        {
            if (Instance != null)
            {
                LogErrorFunc("NetSyncComponentSession is already initialized.");
                return;
            }

            Instance = this;
        }

        public void AddComponent()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            AddComponent(rootObjects);
        }

        public void AddComponent(GameObject[] rootObjects)
        {
            for (int i = 0; i < rootObjects.Length; i++)
            {
                AddComponent(rootObjects[i]);
            }
        }

        public void AddComponent(GameObject rootObject)
        {
            Component[] components = rootObject.GetComponentsInChildren<Component>(true);
            AddComponentAsNetSyncs(components);
        }

        public void AddComponentAsNetSyncs(Component[] components)
        {
            foreach (var component in components)
            {
                if(component != null)
                {
                    Type type = component.GetType();
                    AddComponentAsNetSyncSubspaceObject(component, type);
                }
            }
        }

        public void AddComponentAsNetSyncSubspaceObject(Component component, Type type)
        {
            if (Attribute.IsDefined(type, typeof(NetSyncSubspaceObjectPack)) && typeof(INetSyncSubspaceObject).IsAssignableFrom(type))
            {
                GameObject entityObject = component.gameObject;
                int entityID = entityObject.GetInstanceID();
                NetSyncSubspaceObjectAttribute attribute = (NetSyncSubspaceObjectAttribute)Attribute.GetCustomAttribute(type, typeof(NetSyncSubspaceObjectAttribute));
                int entityGroupID = attribute.NetSyncGroupID;
                
                if (!_syncSubspaceObjectDict_Locals.ContainsKey(entityID))
                {
                    NetSyncSubspaceObjectPack pack = new NetSyncSubspaceObjectPack(entityID, entityGroupID, entityObject);
                    INetSyncSubspaceObject? meta = component as INetSyncSubspaceObject;
                    if (meta != null)
                    {

                    }
                    _syncSubspaceObjectDict_Locals.TryAdd(entityID, pack);
                }
            }
        }

        public void SyncSubspaceObject(GameObject gameObject)
        {
            int entityID = gameObject.GetInstanceID();
            SyncSubspaceObject(entityID);
        }

        public void SyncSubspaceObject(int entityID)
        {
            if (_syncSubspaceObjectDict_Locals.TryGetValue(entityID, out NetSyncSubspaceObjectPack subspaceObject))
            {
                
            }
        }

        public void SyncSubspaceObjectAll()
        {

        }
    }
}
