using SangoUtils_Server.Core;
using SangoUtils_Server.Logger;
using SangoUtils_Server.Net;

namespace SangoUtils_Server
{
    public class DefaultNetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
        {
            SangoLogger.Error("A strange message Received.");
        }
    }
}
