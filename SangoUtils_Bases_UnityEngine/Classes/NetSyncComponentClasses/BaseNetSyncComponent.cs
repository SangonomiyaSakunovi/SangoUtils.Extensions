using System;
using UnityEngine;

namespace SangoUtils.Bases_Unity.NetSyncs
{
    public class BaseNetSyncComponent
    {
        public int EntityID { get; protected set; } = 0;
        public int EntityGroupID { get; protected set; } = 0;
        public GameObject? EntityObject { get; protected set; }

        public Action? OnEntityAwake { get; set; }
        public Action? OnEntityEnable { get; set; }
        public Action? OnEntityDisable { get; set; }
        public Action? OnEntityDestroy { get; set; }

        public Action<object[]>? OnEntityMessage { get; set; }
    }
}
