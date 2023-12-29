using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoUtils_Common;

namespace SangoUtils_Server
{
    public class LoginNetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
        {
            LoginReqInfo? loginReqInfo = DeJsonString<LoginReqInfo>(message);
            if (loginReqInfo != null)
            {
                LoginResCode loginResCode = LoginSystem.Instance.GetLoginRes(loginReqInfo);
                LoginRspInfo loginRspInfo = new(loginResCode);
                string loginResJson = SetJsonString(loginRspInfo);
                peer.SendOperationResponse(NetOperationCode, loginResJson);
            }
            else
            {
                SangoLogger.Warning("LoginReqData cannot DeJsonString.");
            }
        }
    }
}
