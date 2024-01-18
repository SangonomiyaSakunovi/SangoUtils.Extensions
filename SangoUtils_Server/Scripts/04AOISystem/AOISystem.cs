using SangoNetProtol;
using SangoUtils_Server_Scripts;
using SangoUtils_Server_Scripts.AOI;
using SangoUtils_Server_Scripts.Net;
using SangoUtils_Common.Config;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server_App
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
