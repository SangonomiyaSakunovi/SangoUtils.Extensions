using System;
using UnityEngine;

namespace SangoUtils.Bases_Unity.InteractableObjects
{
    public class BaseInteractableObjectPack
    {
        public int EntityID { get; protected set; } = 0;
        public int EntityGroupID { get; protected set; } = 0;
        public GameObject? EntityObject { get; protected set; }

        public Action? OnPressed { get; set; }
        public Action? OnReleased { get; set; }
    }
}
