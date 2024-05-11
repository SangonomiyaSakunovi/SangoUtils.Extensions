using UnityEngine;

namespace SangoUtils.Bases_Unity.NetSyncs
{
    public class NetSyncComponentPack : BaseNetSyncComponentPack
    {
        public NetSyncComponentPack(int entityID, int entityGroupID, GameObject entityObject)
        {
            this.EntityID = entityID;
            this.EntityGroupID = entityGroupID;
            this.EntityObject = entityObject;
        }
    }

    public class NetSyncSubspaceObjectPack : BaseNetSyncComponentPack
    {
        public NetSyncSubspaceObjectPack(int entityID, int entityGroupID, GameObject entityObject)
        {
            this.EntityID = entityID;
            this.EntityGroupID = entityGroupID;
            this.EntityObject = entityObject;
        }
    }
}
