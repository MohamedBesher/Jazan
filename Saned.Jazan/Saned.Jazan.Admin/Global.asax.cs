using Microsoft.Practices.Unity;
using Saned.Jazan.Data.Core;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Saned.Jazan.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            container.RegisterType<INewsRepository, NewsRepository>()
                     .RegisterType<ICategoryRepository, CategoryRepository>()
                     .RegisterType<IDALContext, DALContext>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}