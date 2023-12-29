using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoUtils_Common.Config;
using SangoUtils_Server.Config;

namespace SangoUtils_Server
{
    public class SangoServerRoot : BaseRoot<SangoServerRoot>
    {
        public override void OnInit()
        {
            SangoLogger.InitLogger(SangoSystemConfig.LoggerConfig_Sango);

            NetService.Instance.OnInit();
            LoginSystem.Instance.OnInit();

            InitSystem();

            AOITest();
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

        private void AOITest()
        {
            AOISystem.Instance.OnInit();
        }
    }
}
