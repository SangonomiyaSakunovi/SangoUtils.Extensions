using System;

namespace SangoUtils.Bases_Unity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class InteractableObjectAttribute : Attribute
    {
        public int InteractableObjectGroupID { get; private set; }

        public InteractableObjectAttribute(int interactableObjectGroupID = 0)
        {
            InteractableObjectGroupID = interactableObjectGroupID;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GrabInteractableObjectAttribute : InteractableObjectAttribute
    {
        public GrabInteractableObjectAttribute(int grabObjectGroupID = 0) : base(grabObjectGroupID)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PressedInteractableObjectAttribute : InteractableObjectAttribute
    {
        public PressedInteractableObjectAttribute(int pressedObjectGroupID = 0) : base(pressedObjectGroupID)
        {

        }
    }
}
