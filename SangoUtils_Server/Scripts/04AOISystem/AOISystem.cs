using SangoNetProtol;
using SangoScripts_Server;
using SangoScripts_Server.AOI;
using SangoScripts_Server.Net;
using SangoUtils_Common.Config;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class AOISystem : BaseSystem<AOISystem>
    {
        private AOIIOCPHandler? _aoiNetHandler;
        private AOIIOCPController? _aoiNetController;

        public override void OnInit()
        {
            base.OnInit();
            _aoiNetHandler = IOCPService.Instance.GetNetHandler<AOIIOCPHandler>(NetOperationCode.Aoi);
            _aoiNetController = IOCPService.Instance.GetNetController<AOIIOCPController>(NetOperationCode.Aoi);
            Test();
        }

        public void Test()
        {
            SceneTestMain.Instance.SetConfig(SangoCommonConfig.SceneTestMainConfig);
            SceneTestMain.Instance.OnInit();
        }

        public void OnPlayerEntityEnterInSceneTestMain(BaseObjectEntity entity)
        {
            SceneTestMain.Instance.OnPlayerEntityEnter(entity);
        }

        public void OnPlayerEntityMoveInSceneTestMain(AOIActiveMoveEntity activeMoveEntity)
        {
            SceneTestMain.Instance.OnEntityMove(activeMoveEntity);
        }

        public void SendAOIEventMessage(BaseObjectEntity entity, byte[] message)
        {
            if (entity.PlayerState == PlayerState.Online)
            {
                _aoiNetController?.SendAOIEventMessage(entity,message);
            }
        }
    }
}
