using SangoNetProtol;
using SangoScripts_Server.AOI;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoScripts_Server.Utils;
using SangoUtils_Common.Messages;

namespace SangoScripts_Server
{
    public abstract class BaseObjectEntity(string entityID, Transform transform, IOCPClientPeer clientPeer, AOIEntityType entityType)
    {
        public IOCPClientPeer ClientPeer { get; private set; } = clientPeer;
        public string EntityID { get; private set; } = entityID;
        public Transform Transform { get; set; } = transform;
        public Transform TransformLast { get; set; } = transform;
        public PlayerState PlayerState { get; set; }
        public AOIEntityType AOIEntityType { get; private set; } = entityType;

        public AOIEntity? AOIEntity { get; set; }

        public void OnEnterToScene<T>(BaseScene<T> scene) where T : class, new()
        {
            PlayerState = PlayerState.Online;
            SangoLogger.Processing($"EntityID: [ {EntityID} ] is enter to map.");
        }

        public void OnMoveInMap(AOIEventMessage message)
        {
            if (PlayerState == PlayerState.Online)
            {
                string messageJson = JsonUtils.SetJsonString(message);
                ClientPeer.SendEvent(NetOperationCode.Aoi, messageJson);
            }       
        }

        public void OnMoveInMap(byte[] message)
        {
            ClientPeer.SendPackMessage(message);
        }

        public void OnExitFromScene()
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
