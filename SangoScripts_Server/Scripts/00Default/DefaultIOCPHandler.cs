using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;

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
