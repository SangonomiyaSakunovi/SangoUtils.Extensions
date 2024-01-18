using SangoNetProtol;
using SangoUtils_Server_Scripts.AOI;
using SangoUtils_Server_Scripts.Net;
using SangoUtils_Server_Scripts.Utils;
using SangoUtils_Common.Messages;
using SangoUtils_Logger;

namespace SangoUtils_Server_Scripts
{
    public abstract class BaseObjectEntity(string entityID, TransformData transform, IOCPClientPeer clientPeer, AOIEntityType entityType)
    {
        public IOCPClientPeer ClientPeer { get; private set; } = clientPeer;
        public string EntityID { get; private set; } = entityID;
        public TransformData Transform { get; set; } = transform;
        public TransformData TransformLast { get; set; } = transform;
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
            ClientPeer.SendPacked(message);
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
