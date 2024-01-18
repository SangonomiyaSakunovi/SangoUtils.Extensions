using SangoUtils_Server_Scripts;

namespace SangoUtils_Server_App
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
