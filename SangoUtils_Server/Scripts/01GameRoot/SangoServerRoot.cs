using SangoUtils_Server_Scripts;
using SangoUtils_Server_Scripts.Net;
using SangoUtils_Logger;
using SangoUtils_Server_App.Config;

namespace SangoUtils_Server_App
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
            IOCPService.Instance.CloseClientInstance();
        }

        private void InitConfig()
        {
            SangoLogger.InitLogger(SangoSystemConfig.LoggerConfig_Sango);
        }

        private void InitService()
        {
            IOCPService.Instance.OnInit();
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
