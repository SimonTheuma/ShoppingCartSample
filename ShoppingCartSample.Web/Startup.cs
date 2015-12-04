using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;
using ShoppingCartSample.App_Start;
using ShoppingCartSample.Authentication.Configuration;
using ShoppingCartSample.Data.Contexts;

[assembly: OwinStartup(typeof(ShoppingCartSample.Startup))]

namespace ShoppingCartSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthorizationStartup.ConfigureAuth(app);           

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterTypes(new UnityContainer());
        }
    }
}
