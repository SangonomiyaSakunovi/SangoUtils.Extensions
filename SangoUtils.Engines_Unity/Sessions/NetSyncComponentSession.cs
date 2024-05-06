using SangoUtils.Bases_Unity;
using SangoUtils.Bases_Unity.NetSyncs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace SangoUtils.Engines_Unity
{
    public class NetSyncComponentSession : MonoBehaviour
    {
#pragma warning disable CS8618
        public static NetSyncComponentSession Instance { get; private set; }
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        private readonly ConcurrentDictionary<int, NetSyncSubspaceObject> _syncSubspaceObjectDict = new ConcurrentDictionary<int, NetSyncSubspaceObject>();

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

        public void AddComponent(Component[] components)
        {
            foreach (var component in components)
            {
                Type type = component.GetType();
                if(Attribute.IsDefined(type,typeof(NetSyncSubspaceObject)) && typeof(INetSyncSubspaceObject).IsAssignableFrom(type))
                {
                    int entityID = type.GetHashCode();
                    NetSyncSubspaceObjectAttribute attribute = (NetSyncSubspaceObjectAttribute)Attribute.GetCustomAttribute(type, typeof(NetSyncSubspaceObjectAttribute));
                }
            }
        }
    }
}
