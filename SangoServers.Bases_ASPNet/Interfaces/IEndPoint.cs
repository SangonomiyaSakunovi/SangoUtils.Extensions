using Microsoft.AspNetCore.Builder;

namespace SangoUtils.Behaviours_ASPNet
{
    public interface IEndPoint
    {
        void MapPoint(WebApplication app);
    }
}
