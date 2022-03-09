
using System.Web.Mvc;
using System.Web.Routing;

namespace Ks.ConsultasIntegracoes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{id}", (object)new
            {
                controller = "Login",
                action = "Logar",
                id = UrlParameter.Optional
            });
        }
    }
}
