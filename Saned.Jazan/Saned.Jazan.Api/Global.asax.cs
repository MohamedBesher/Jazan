using Microsoft.Practices.Unity;
using Saned.Jazan.Data.Core;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static Saned.Jazan.Api.WebApiConfig;

namespace Saned.Jazan.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();


            // Other Web API configuration not shown.


            //var container = new UnityContainer();
            //container.RegisterType<INewsRepository, NewsRepository>()
            //         .RegisterType<ICategoryRepository, CategoryRepository>()
            //         .RegisterType<IDALContext, DALContext>();
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}
