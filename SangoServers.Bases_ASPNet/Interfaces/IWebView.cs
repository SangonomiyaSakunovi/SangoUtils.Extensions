using Microsoft.AspNetCore.Builder;

namespace SangoServers.Bases_ASPNet
{
    public interface IWebView
    {
        void MapView(WebApplication app);
    }
}
