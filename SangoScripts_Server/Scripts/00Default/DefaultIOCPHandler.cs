using SangoScripts_Server.Net;
using SangoUtils_Logger;

namespace SangoScripts_Server
{
    public class DefaultIOCPHandler : BaseIOCPNetHandler
    {
        public override void OnOperationRequest(string message, IOCPClientPeer peer)
        {
            SangoLogger.Error("A strange message Received.");
        }
    }
}
