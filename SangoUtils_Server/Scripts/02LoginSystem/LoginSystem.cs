using SangoNetProtol;
using SangoScripts_Server;
using SangoScripts_Server.Net;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class LoginSystem : BaseSystem<LoginSystem>
    {
        private LoginNetHandler? _loginNetHandler;

        public override void OnInit()
        {
            base.OnInit();
            _loginNetHandler = NetService.Instance.GetNetHandler<LoginNetHandler>(NetOperationCode.Login);
        }

        public LoginResCode GetLoginRes(LoginReqMessage loginReqMessage)
        {
            LoginResCode loginResCode = LoginResCode.None;
            switch (loginReqMessage.LoginMode)
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
