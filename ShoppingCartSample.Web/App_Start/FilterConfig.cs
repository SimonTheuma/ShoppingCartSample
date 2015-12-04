using System.Web;
using System.Web.Mvc;
using ShoppingCartSample.Attributes;

namespace ShoppingCartSample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JsonNetFilterAttribute());
        }
    }
}
