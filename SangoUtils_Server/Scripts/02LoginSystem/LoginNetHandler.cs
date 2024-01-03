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
            //if (loginReqMessage != null)
            //{
            //    LoginResCode loginResCode = LoginSystem.Instance.GetLoginRes(loginReqMessage);
            //    LoginRspMessage loginRspMessage = new(loginResCode);
            //    string loginResJson = SetJsonString(loginRspMessage);
            //    peer.SendOperationResponse(NetOperationCode, loginResJson);
            //}
            //else
            //{
            //    SangoLogger.Warning("LoginReqData cannot DeJsonString.");
            //}
        }
    }
}
