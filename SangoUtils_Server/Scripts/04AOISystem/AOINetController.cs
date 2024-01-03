using SangoScripts_Server;
using SangoUtils_Common;
using SangoUtils_Common.Config;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class AOINetController : BaseNetController
    {
        private SunUpF4LobbyMap _sunUpF4LobbyMap = new();

        public override void DefaultOperationEvent()
        {
            
        }

        public void Test()
        {
            _sunUpF4LobbyMap.SetConfig(SangoCommonConfig.SunUpF4LobbyMapConfig);
            _sunUpF4LobbyMap.OnInit();
        }

        public void UpdateAOIPos(AOIActiveMoveEntity activeMoveEntity)
        {
            _sunUpF4LobbyMap.OnEntityMove(activeMoveEntity);
        }

        public void ExitAOIPos(AOIActiveMoveEntity activeMoveEntity)
        {
            _sunUpF4LobbyMap.OnEntityExit(activeMoveEntity.EntityID);
        }
    }
}
