namespace SangoUtils.Bases_Unity.Trackables
{
    public interface ITrackableComponent
    {
        void OnTrackableAwake();

        void OnTrackbaleEnable();

        void OnTrackableDisable();

        void OnTrackableDestroy();

        void OnTrackableMessage(params object[] messages);
    }

    public interface ITrackableWindow : ITrackableComponent
    {
        
    }

    public interface ITrackablePanel : ITrackableComponent
    {
        
    }
}
