using SangoScripts_Server;
using SangoScripts_Server.AOI;
using SangoScripts_Server.Net;

namespace SangoUtils_Server
{
    public class PlayerEntity(string entityID, Transform transform, ClientPeer clientPeer) : BaseObjectEntity(entityID, transform, clientPeer, AOIEntityType.Client)
    {

    }
}
