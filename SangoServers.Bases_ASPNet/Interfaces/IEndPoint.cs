using Microsoft.AspNetCore.Builder;

namespace SangoServers.Bases_ASPNet
{
    public interface IEndPoint
    {
        void MapPoint(WebApplication app);
    }
}
