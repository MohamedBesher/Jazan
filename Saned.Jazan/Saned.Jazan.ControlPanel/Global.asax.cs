using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using Saned.Common.Comments.Repository;
using Saned.Jazan.Data.Core;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;

namespace Saned.Jazan.ControlPanel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
         

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();

            //BundleTable.EnableOptimizations = true;

            var container = new UnityContainer();
            container.RegisterType<INewsRepository, NewsRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ICommentRepositoryAsync, CommentRepositoryAsync>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IUsersRepository, UsersRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ICategoryRepository, CategoryRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IAdvertisementRepository, AdvertisementRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IAdvertisementImageRepository, AdvertisementImageRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IAdvertisementFeatureRepository, AdvertisementFeatureRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IPackageRepository, PackageRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IFeatureRepository, FeatureRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<IMobileSettingRepository, MobileSettingRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ICulturalCompetitionAnswersRepository, CulturalCompetitionAnswersRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ICulturalCompetitionQuestionRepository, CulturalCompetitionQuestionRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ICulturalCompetitionQuestionSponsorsRepository, CulturalCompetitionQuestionSponsorsRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ITouristVisitRepository, TouristVisitRepository>().RegisterType<IDALContext, DALContext>();
            container.RegisterType<ITouristVisitImageRepository, TouristVisitImageRepository>().RegisterType<IDALContext, DALContext>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

          

        }
}
}
