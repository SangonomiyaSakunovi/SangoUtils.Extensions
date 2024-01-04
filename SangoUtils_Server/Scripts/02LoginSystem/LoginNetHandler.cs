using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class LoginNetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
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
