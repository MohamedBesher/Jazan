using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Microsoft.Practices.Unity;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using System.Net.Http.Formatting;
using System.Web.Http.Dependencies;
using AutoMapper;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;


namespace Saned.Jazan.Api
{
  
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            var container = new UnityContainer();
            container.RegisterType<INewsRepository, NewsRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ICategoryRepository, CategoryRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IAdvertisementRepository, AdvertisementRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IAdvertisementImageRepository, AdvertisementImageRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IAdvertisementFeatureRepository, AdvertisementFeatureRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IPackageRepository, PackageRepository>(new PerThreadLifetimeManager());
            container.RegisterType<IMobileSettingRepository, MobileSettingRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ICulturalCompetitionAnswersRepository, CulturalCompetitionAnswersRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ICulturalCompetitionQuestionRepository, CulturalCompetitionQuestionRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ICulturalCompetitionQuestionSponsorsRepository, CulturalCompetitionQuestionSponsorsRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ITouristVisitRepository, TouristVisitRepository>(new PerThreadLifetimeManager());
            container.RegisterType<ITouristVisitImageRepository, TouristVisitImageRepository>(new PerThreadLifetimeManager());
            config.DependencyResolver = new UnityDependencyResolver(container);
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ClientViewModel, RegisterDataDto>();
                CreateMap<Saned.Jazan.Api.Models.AdvertisementViewModel, Advertisement>()
                    .ForMember(x => x.AdvertisementImages, option => option.Ignore());

                CreateMap<TouristVisitViewModel, TouristVisit>()
                    .ForMember(x => x.TouristVisitImages, option => option.Ignore());
            }
        }

        public class AutoMapperConfiguration
        {
            public static void Configure()
            {
                AutoMapper.Mapper.Initialize(x =>
                {
                    x.AddProfile<MappingProfile>();
                });
            }
        }
    }
}
