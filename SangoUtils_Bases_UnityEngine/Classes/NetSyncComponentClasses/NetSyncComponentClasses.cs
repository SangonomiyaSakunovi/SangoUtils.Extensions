using UnityEngine;

namespace SangoUtils.Bases_Unity.NetSyncs
{
    public class NetSyncComponent : BaseNetSyncComponent
    {
        public NetSyncComponent(int entityID, int entityGroupID, GameObject entityObject)
        {
            this.EntityID = entityID;
            this.EntityGroupID = entityGroupID;
            this.EntityObject = entityObject;
        }
    }

    public class NetSyncSubspaceObject : BaseNetSyncComponent
    {
        public NetSyncSubspaceObject(int entityID, int entityGroupID, GameObject entityObject)
        {
            this.EntityID = entityID;
            this.EntityGroupID = entityGroupID;
            this.EntityObject = entityObject;
        }
    }
}
