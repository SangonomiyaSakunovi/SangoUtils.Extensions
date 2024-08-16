using UnityEngine;

namespace SangoUtils.Behaviours_Unity.RecognizableObjects
{
    public class MarkerRecognizableObjectPack : BaseRecognizableObjectPack
    {
        public MarkerRecognizableObjectPack(int entityID, int entityGroupID, GameObject entityObject)
        {
            EntityID = entityID;
            EntityGroupID = entityGroupID;
            EntityObject = entityObject;
        }
    }
}
