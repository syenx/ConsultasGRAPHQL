
using System.Web.Mvc;

namespace Ks.ConsultasIntegracoes
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) => filters.Add((object)new HandleErrorAttribute());
    }
}
