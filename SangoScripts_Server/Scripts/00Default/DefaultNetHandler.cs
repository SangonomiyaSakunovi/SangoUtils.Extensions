using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;

namespace SangoScripts_Server
{
    public class DefaultNetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
        {
            SangoLogger.Error("A strange message Received.");
        }
    }
}
