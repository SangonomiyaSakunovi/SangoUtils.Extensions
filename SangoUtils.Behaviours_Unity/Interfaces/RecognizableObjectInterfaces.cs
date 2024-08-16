namespace SangoUtils.Behaviours_Unity.RecognizableObjects
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
