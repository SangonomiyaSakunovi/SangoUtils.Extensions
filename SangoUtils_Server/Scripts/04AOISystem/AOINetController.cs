using SangoScripts_Server;
using SangoUtils_Common;
using SangoUtils_Common.Config;

namespace SangoUtils_Server
{
    public class AOINetController : BaseNetController
    {
        SunUpF4LobbyMap sunUpF4LobbyMap = new SunUpF4LobbyMap();


        public override void DefaultOperationEvent()
        {
            
        }

        public void Test()
        {
            sunUpF4LobbyMap.SetConfig(SangoCommonConfig.SunUpF4LobbyMapConfig);
            sunUpF4LobbyMap.OnInit();
        }

        public void UpdateAOIPos(SyncTransformInfo syncTransformInfo)
        {
            sunUpF4LobbyMap.OnEntityMove(syncTransformInfo);
        }

        public void ExitAOIPos(SyncTransformInfo syncTransformInfo)
        {
            sunUpF4LobbyMap.OnEntityExit(syncTransformInfo.EntityID);
        }
    }
}
