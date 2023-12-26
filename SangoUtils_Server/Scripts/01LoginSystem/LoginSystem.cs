using SangoNetProtol;
using SangoUtils_Common;
using SangoUtils_Server.Core;
using SangoUtils_Server.Net;

namespace SangoUtils_Server
{
    public class LoginSystem : BaseSystem<LoginSystem>
    {
        private LoginNetHandler? loginNetHandler;

        public override void OnInit()
        {
            base.OnInit();
            loginNetHandler = NetService.Instance.GetNetHandler<LoginNetHandler>(NetOperationCode.Login);
        }

        public LoginResCode GetLoginRes(LoginReqInfo loginReqInfo)
        {
            LoginResCode loginResCode = LoginResCode.None;
            switch (loginReqInfo.LoginMode)
            {
                case LoginMode.Guest:
                    loginResCode = LoginResCode.LoginSuccess;
                    break;
                case LoginMode.UIDAndPassword:
                    //TODO
                    break;
            }
            return loginResCode;
        }
    }
}
