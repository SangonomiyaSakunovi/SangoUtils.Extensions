using System;
using UnityEngine;

namespace SangoUtils.Bases_Unity.RecognizableObjects
{
    public class BaseRecognizableObjectPack
    {
        public int EntityID { get; protected set; } = 0;
        public int EntityGroupID { get; protected set; } = 0;
        public GameObject? EntityObject { get; protected set; }

        public Action? OnRecognized { get; set; }
        public Action? OnLost { get; set; }
    }
}
