using UnityEngine;

namespace SangoUtils.Bases_Unity.InteractableObjects
{
    public class GrabInteractableObjectPack : BaseInteractableObjectPack
    {
        public GrabInteractableObjectPack(int entityID, int entityGroupID, GameObject entityObject)
        {
            this.EntityID = entityID;
            this.EntityGroupID = entityGroupID;
            this.EntityObject = entityObject;
        }
    }

    public class PressedInteractableObjectPack : BaseInteractableObjectPack
    {
        public PressedInteractableObjectPack(int entityID, int entityGroupID, GameObject entityObject)
        {
            this.EntityID = entityID;
            this.EntityGroupID = entityGroupID;
            this.EntityObject = entityObject;
        }
    }
}
