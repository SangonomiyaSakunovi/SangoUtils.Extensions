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
        private AOINetHandler? _aoiNetHandler;
        private AOINetController? _aoiNetController;

        public override void OnInit()
        {
            base.OnInit();
            _aoiNetHandler = NetService.Instance.GetNetHandler<AOINetHandler>(NetOperationCode.Aoi);
            _aoiNetController = NetService.Instance.GetNetController<AOINetController>(NetOperationCode.Aoi);
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
