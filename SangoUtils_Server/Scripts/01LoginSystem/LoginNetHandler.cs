using SangoNetProtol;
using SangoUtils_Common;
using SangoUtils_Server.Core;
using SangoUtils_Server.Logger;
using SangoUtils_Server.Net;

namespace SangoUtils_Server
{
    public class LoginNetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
        {
            LoginReqData? loginReqData = DeJsonString<LoginReqData>(message);
            if (loginReqData != null )
            {
                LoginReqInfo loginReqInfo = new(loginReqData.LoginMode, loginReqData.UID, loginReqData.Password); 
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
