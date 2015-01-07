using System.Web.Http;

namespace Server.Configuration
{
    internal static class Config
    {
        public static void Register(HttpConfiguration pConfig)
        {
            pConfig.MapHttpAttributeRoutes();
            pConfig.EnsureInitialized();
        }
    }
}