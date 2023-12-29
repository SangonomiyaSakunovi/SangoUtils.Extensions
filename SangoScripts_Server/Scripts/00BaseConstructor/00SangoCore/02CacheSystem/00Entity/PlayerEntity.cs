using SangoScripts_Server.AOI;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Map;
using SangoScripts_Server.Net;
using SangoScripts_Server.Utils;
using SangoUtils_Common;

namespace SangoScripts_Server.Cache
{
    public class PlayerEntity(string entityID, TransformInfo transformInfo, ClientPeer clientPeer)
    {
        public ClientPeer ClientPeer { get; private set; } = clientPeer;
        public string EntityID { get; private set; } = entityID;
        public TransformInfo TransformInfo { get; set; } = transformInfo;
        public PlayerState PlayerState { get; set; }

        public AOIEntity? AOIEntity { get; set; }

        public void OnEnterToMap(MapBaseStage map)
        {
            PlayerState = PlayerState.Online;
            SangoLogger.Processing($"EntityID: [ {EntityID} ] is enter to map.");
        }

        public void OnUpdateInMap(AOIMessage message)
        {
            if (PlayerState == PlayerState.Online)
            {
                string messageJson = JsonUtils.SetJsonString(message);
                ClientPeer.SendEvent(SangoNetProtol.NetOperationCode.Aoi, messageJson);
            }
        }

        public void OnExitFromMap()
        {
            PlayerState = PlayerState.Offline;
            SangoLogger.Processing($"EntityID: [ {EntityID} ] is exit from map.");
        }
    }

    public enum PlayerState
    {
        None = 0,
        Online = 1,
        Offline = 2,
    }
}
