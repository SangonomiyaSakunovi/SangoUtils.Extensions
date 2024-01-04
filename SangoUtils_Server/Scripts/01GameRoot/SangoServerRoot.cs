using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoUtils_Server.Config;

namespace SangoUtils_Server
{
    public class SangoServerRoot : BaseRoot<SangoServerRoot>
    {
        public override void OnInit()
        {
            InitConfig();
            InitScene();
            InitService();
            InitSystem();
            AOITest();
        }

        public override void Update()
        {
            base.Update();
            SceneService.Instance.Update();
        }

        public override void OnDispose()
        {
            base.OnDispose();
            NetService.Instance.CloseClientInstance();
        }

        private void InitConfig()
        {
            SangoLogger.InitLogger(SangoSystemConfig.LoggerConfig_Sango);
        }

        private void InitService()
        {
            NetService.Instance.OnInit();
        }

        private void InitSystem()
        {
            LoginSystem.Instance.OnInit();
        }

        private void InitScene()
        {
            SceneTestMain.Instance.OnInit();
        }

        private void AOITest()
        {
            AOISystem.Instance.OnInit();
        }
    }
}
