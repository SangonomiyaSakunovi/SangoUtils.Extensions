using SangoNetProtol;
using SangoScripts_Server;
using SangoScripts_Server.Net;
using SangoUtils_Common;

namespace SangoUtils_Server
{
    public class AOISystem : BaseSystem<AOISystem>
    {
        private AOINetController? _aoiNetController;

        public override void OnInit()
        {
            base.OnInit();
            _aoiNetController = NetService.Instance.GetNetController<AOINetController>(NetOperationCode.Aoi);
            Test();
        }

        public void Test()
        {
            _aoiNetController.Test();
        }

        public void UpdateAOIPos(SyncTransformInfo syncTransformInfo)
        {
            _aoiNetController.UpdateAOIPos(syncTransformInfo);
        }

        public void ExitAOIPos(SyncTransformInfo syncTransformInfo)
        {
            _aoiNetController.ExitAOIPos(syncTransformInfo);
        }
    }
}
