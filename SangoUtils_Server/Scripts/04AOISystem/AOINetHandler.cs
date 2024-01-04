using SangoScripts_Server;
using SangoScripts_Server.Logger;
using SangoScripts_Server.Net;
using SangoUtils_Common.Messages;

namespace SangoUtils_Server
{
    public class AOINetHandler : BaseNetHandler
    {
        public override void OnOperationRequest(string message, ClientPeer peer)
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
            SceneTestMain.Instance.OnEntityMove(activeMoveEntity);
        }
    }
}
