using UnityEngine;

namespace SangoUtils.Bases_Unity
{
    internal class TrackabeWindow : BaseTrackableClass
    {
        public TrackabeWindow(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }

    internal class TrackablePanel : BaseTrackableClass
    {
        public TrackablePanel(int trackableID, int trackableGroupID, GameObject trackableObject)
        {
            this.TrackableID = trackableID;
            this.TrackableGroupID = trackableGroupID;
            this.TrackableObject = trackableObject;
        }
    }
}
