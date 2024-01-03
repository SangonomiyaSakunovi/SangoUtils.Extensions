using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class AOINetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
        {
            SangoLogger.Log(message);
            AOIReqMessage? aoiReqMessage = DeJsonString<AOIReqMessage>(message);
            if (aoiReqMessage != null)
            {

            }
        }
    }
}
