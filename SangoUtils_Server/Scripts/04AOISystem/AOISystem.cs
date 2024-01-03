using SangoNetProtol;
using SangoScripts_Server;
using SangoScripts_Server.Net;
using SangoUtils_Common;
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
            _aoiNetController?.Test();
        }

        public void UpdateAOIPos(AOIActiveMoveEntity activeMoveEntity)
        {
            _aoiNetController?.UpdateAOIPos(activeMoveEntity);
        }

        public void ExitAOIPos(AOIActiveMoveEntity activeMoveEntity)
        {
            _aoiNetController?.ExitAOIPos(activeMoveEntity);
        }
    }
}
