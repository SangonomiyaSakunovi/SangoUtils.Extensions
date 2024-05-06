using System;
using UnityEngine;

namespace SangoUtils.Bases_Unity.Trackables
{
    public class BaseTrackableComponent
    {
        public int TrackableID { get; protected set; } = 0;
        public int TrackableGroupID { get; protected set; } = 0;
        public GameObject? TrackableObject { get; protected set; }

        public Action? OnTrackableAwake { get; set; }
        public Action? OnTrackbaleEnable { get; set; }
        public Action? OnTrackableDisable { get; set; }
        public Action? OnTrackableDestroy { get; set; }
        
        public Action<object[]>? OnTrackableMessage { get; set; }
    }
}
