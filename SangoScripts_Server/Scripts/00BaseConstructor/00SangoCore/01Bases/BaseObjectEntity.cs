using SangoNetProtol;
using SangoScripts_Server.AOI;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoScripts_Server.Utils;
using SangoUtils_Common.Messages;

namespace SangoScripts_Server
{
    public class BaseObjectEntity(string entityID, Transform transform, ClientPeer clientPeer, AOIEntityType entityType)
    {
        public ClientPeer ClientPeer { get; private set; } = clientPeer;
        public string EntityID { get; private set; } = entityID;
        public Transform Transform { get; set; } = transform;
        public PlayerState PlayerState { get; set; }
        public AOIEntityType AOIEntityType { get; private set; } = entityType;

        public AOIEntity? AOIEntity { get; set; }

        public void OnEnterToMap<T>(BaseScene<T> scene) where T : class, new()
        {
            PlayerState = PlayerState.Online;
            SangoLogger.Processing($"EntityID: [ {EntityID} ] is enter to map.");
        }

        public void OnUpdateInMap(AOIEventMessage message)
        {
            if (PlayerState == PlayerState.Online)
            {
                string messageJson = JsonUtils.SetJsonString(message);
                ClientPeer.SendEvent(NetOperationCode.Aoi, messageJson);
            }
        }

        public void OnUpdateInMap(byte[] bytes)
        {
            if (PlayerState == PlayerState.Online)
            {
                ClientPeer.SendPackMessage(bytes);
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
