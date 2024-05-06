using UnityEngine;

namespace SangoUtils.Bases_Unity.Trackables
{
    public class TrackableComponent : BaseTrackableComponent
    {
        public TrackableComponent(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }

    public class TrackabeWindow : BaseTrackableComponent
    {
        public TrackabeWindow(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }

    public class TrackablePanel : BaseTrackableComponent
    {
        public TrackablePanel(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }
}
