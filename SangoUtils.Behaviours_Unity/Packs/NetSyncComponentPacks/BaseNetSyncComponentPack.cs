using System;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.NetSyncs
{
    public class BaseNetSyncComponentPack
    {
        public int EntityID { get; protected set; } = 0;
        public int EntityGroupID { get; protected set; } = 0;
        public GameObject? EntityObject { get; protected set; }
    }
}
