using SangoUtils_Server_Scripts.Net;
using SangoUtils_Logger;

namespace SangoUtils_Server_Scripts
{
    public class DefaultIOCPHandler : BaseIOCPNetHandler
    {
        public override void OnOperationRequest(string message, IOCPClientPeer peer)
        {
            SangoLogger.Error("A strange message Received: "+ message);
        }
    }
}
