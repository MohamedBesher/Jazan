using System.Linq;
using System.Web.Http;
using AutoMapper;
using Microsoft.Practices.Unity;
using Saned.Jazan.ControlPanel.ViewModels;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;

namespace Saned.Jazan.ControlPanel
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
          
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompetitionQuestionViewModel, CulturalCompetitionQuestion>();
            CreateMap<CulturalCompetitionQuestion, CompetitionQuestionViewModel>();
            //CreateMap<AdvertisementViewModel, Advertisement>()
            //    .ForMember(x => x.AdvertisementImages, option => option.Ignore());

            //CreateMap<TouristVisitViewModel, TouristVisit>()
            //    .ForMember(x => x.TouristVisitImages, option => option.Ignore());

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
