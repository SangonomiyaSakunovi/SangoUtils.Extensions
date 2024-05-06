using System;

namespace SangoUtils.Bases_Unity.Trackables
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TrackableComponentAttribute : Attribute
    {
        public int TrackableGroupID { get; private set; }

        public TrackableComponentAttribute(int trackableGroupID = 0)
        {
            TrackableGroupID = trackableGroupID;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TrackableWindowAttribute : TrackableComponentAttribute
    {
        public TrackableWindowAttribute(int trackableGroupID = 0) : base(trackableGroupID)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TrackablePanelAttribute : TrackableComponentAttribute
    {
        public TrackablePanelAttribute(int trackableGroupID = 0) : base(trackableGroupID)
        {
        }
    }
}

