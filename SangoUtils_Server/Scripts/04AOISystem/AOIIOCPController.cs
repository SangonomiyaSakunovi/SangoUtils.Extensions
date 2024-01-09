using SangoScripts_Server;

namespace SangoUtils_Server
{
    public class AOIIOCPController : BaseNetController
    {
        public override void DefaultOperationEvent()
        {

        }

        public void SendAOIEventMessage(BaseObjectEntity entity, byte[] message)
        {
            entity.OnMoveInMap(message);
        }
    }
}
