namespace SangoUtils.Bases_Unity
{
    public interface INetSyncComponent
    {
        void OnNetSyncAwake();

        void OnNetSyncEnable();

        void OnNetSyncDisable();

        void OnNetSyncDestroy();

        void OnNetSyncMessage(params object[] messages);
    }
    
    public interface INetSyncSubspaceObject : INetSyncComponent
    {
        
    }
}
