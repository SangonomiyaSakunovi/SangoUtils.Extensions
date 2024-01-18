using SangoUtils_Server_Scripts;
using SangoUtils_Server_Scripts.AOI;
using SangoUtils_Server_Scripts.Net;

namespace SangoUtils_Server_App
{
    public class PlayerEntity(string entityID, TransformData transform, IOCPClientPeer clientPeer) : BaseObjectEntity(entityID, transform, clientPeer, AOIEntityType.Client)
    {

    }
}
