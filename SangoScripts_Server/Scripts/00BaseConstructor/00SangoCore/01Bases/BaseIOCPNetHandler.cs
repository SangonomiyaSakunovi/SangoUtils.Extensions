using SangoUtils_Server_Scripts.Net;

namespace SangoUtils_Server_Scripts
{
    public abstract class BaseIOCPNetHandler : BaseNetHandler
    {
        public abstract void OnOperationRequest(string message, IOCPClientPeer peer);
    }
}
