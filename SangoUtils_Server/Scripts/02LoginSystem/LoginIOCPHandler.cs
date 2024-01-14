using SangoScripts_Server;
using SangoScripts_Server.Net;
using SangoUtils_Common.Messages;
using SangoUtils_Logger;

namespace SangoUtils_Server
{
    public class LoginIOCPHandler : BaseIOCPNetHandler
    {
        public override void OnOperationRequest(string message, IOCPClientPeer peer)
        {
            LoginReqMessage? loginReqMessage = DeJsonString<LoginReqMessage>(message);
            if (loginReqMessage != null)
            {
                LoginResCode loginResCode = LoginSystem.Instance.GetLoginRes(loginReqMessage, peer);
                LoginRspMessage loginRspMessage = new(loginResCode, peer.EntityID);
                string loginResJson = SetJsonString(loginRspMessage);
                peer.SendOperationResponse(NetOperationCode, loginResJson);
            }
            else
            {
                SangoLogger.Warning("LoginReqData cannot DeJsonString.");
            }
        }
    }
}
