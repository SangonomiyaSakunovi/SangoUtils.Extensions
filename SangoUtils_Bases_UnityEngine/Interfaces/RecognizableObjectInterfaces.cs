namespace SangoUtils.Bases_Unity.RecognizableObjects
{
    public interface IRecognizableObject
    {
        void OnRecognized();

        void OnLost();
    }

    public interface IMarkerRecognizableObject : IRecognizableObject
    {

    }
}
