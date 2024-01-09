using SangoScripts_Server.Net;

namespace SangoScripts_Server
{
    public abstract class BaseIOCPNetHandler : BaseNetHandler
    {
        public abstract void OnOperationRequest(string message, IOCPClientPeer peer);
    }
}
