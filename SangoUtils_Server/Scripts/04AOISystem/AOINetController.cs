using SangoScripts_Server;
using SangoUtils_Common;
using SangoUtils_Common.Config;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class AOINetController : BaseNetController
    {
        public override void DefaultOperationEvent()
        {
            
        }

        public void Test()
        {
            SceneTestMain.Instance.SetConfig(SangoCommonConfig.SceneTestMainConfig);
            SceneTestMain.Instance.OnInit();
        }

        public void UpdateAOIPos(AOIActiveMoveEntity activeMoveEntity)
        {
            SceneTestMain.Instance.OnEntityMove(activeMoveEntity);
        }

        public void ExitAOIPos(AOIActiveMoveEntity activeMoveEntity)
        {
            SceneTestMain.Instance.OnEntityExit(activeMoveEntity.EntityID);
        }
    }
}
