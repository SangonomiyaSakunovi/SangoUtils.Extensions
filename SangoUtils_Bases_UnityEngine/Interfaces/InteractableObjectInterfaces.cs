namespace SangoUtils.Bases_Unity.InteractableObjects
{
    public interface IInteractableObject
    {
        void OnPressed();

        void OnReleased();
    }

    public interface IGrabInteractableObject : IInteractableObject
    {

    }

    public interface IPressedInteractableObject : IInteractableObject
    {

    }
}
