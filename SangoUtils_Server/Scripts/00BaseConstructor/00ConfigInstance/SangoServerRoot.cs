using SangoUtils_Server.Config;
using SangoUtils_Server.Core;
using SangoUtils_Server.Logger;
using SangoUtils_Server.Net;

namespace SangoUtils_Server.Program
{
    public class SangoServerRoot : BaseRoot<SangoServerRoot>
    {
        public override void OnInit()
        {
            SangoLogger.InitLogger(SangoSystemConfig.LoggerConfig_Sango);

            NetService.Instance.OnInit();
            LoginSystem.Instance.OnInit();

            InitSystem();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnDispose()
        {
            base.OnDispose();
            NetService.Instance.CloseClientInstance();
        }

        private void InitSystem()
        {
            LoginSystem.Instance.OnInit();
        }
    }
}
