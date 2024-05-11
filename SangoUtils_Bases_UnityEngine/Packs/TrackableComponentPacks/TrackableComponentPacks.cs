using UnityEngine;

namespace SangoUtils.Bases_Unity.Trackables
{
    public class TrackableComponentPack : BaseTrackableComponentPack
    {
        public TrackableComponentPack(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }

    public class TrackabeWindowPack : BaseTrackableComponentPack
    {
        public TrackabeWindowPack(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }

    public class TrackablePanelPack : BaseTrackableComponentPack
    {
        public TrackablePanelPack(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }
}
