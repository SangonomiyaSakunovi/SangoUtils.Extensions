using SangoUtils_Server_Scripts;
using SangoUtils_Server_Scripts.Net;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server_App
{
    public class AOIIOCPHandler : BaseIOCPNetHandler
    {
        public override void OnOperationRequest(string message, IOCPClientPeer peer)
        {
            //SangoLogger.Log(message);
            AOIReqMessage? aoiReqMessage = DeJsonString<AOIReqMessage>(message);
            if (aoiReqMessage != null)
            {
                List<AOIActiveMoveEntity> aoiActiveMoveEntitys = aoiReqMessage.AOIActiveMoveEntitys;
                for (int i = 0; i < aoiActiveMoveEntitys.Count; i++)
                {
                    OnPlayerEntityMoveInSceneTestMain(aoiActiveMoveEntitys[i]);
                }
            }
        }

        private void OnPlayerEntityMoveInSceneTestMain(AOIActiveMoveEntity activeMoveEntity)
        {
            AOISystem.Instance.OnPlayerEntityMoveInSceneTestMain(activeMoveEntity);
        }
    }
}
