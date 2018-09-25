using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

using System.Data.Entity;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/News")]
    public class NewsController : ApiController
    {
        INewsRepository repo;
        public NewsController(INewsRepository INewsRepository)
        {
            this.repo = INewsRepository;
        }

        [HttpPost]
        [Route("GetNews")]
        public IHttpActionResult GetNews(NewsRequestDto request)
        {
            try
            {
                var news = repo.All().OrderBy(x => x.Id).Skip((request.PageNumber - 1) * request.PageSize).OrderByDescending(x => x.PublishingDate)
                     .Take(request.PageSize).ToList();

                return Ok(news);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // for  Debuging purpose in Client only.
            }
        }

        [HttpGet]
        [Route("GetSingleNews/{NewsId}")]
        public IHttpActionResult GetSingleNews(int NewsId)
        {
            try
            {
                if (NewsId == 0)
                {
                    return BadRequest();
                }

                var result = repo.GetNewsById(NewsId);

                if (result != null)
                {
                    return Ok(new
                    {
                        ImagePath = result.NewsImages.FirstOrDefault() != null ?
                        result.NewsImages.FirstOrDefault().ImagePath : string.Empty,
                        Title = result.Title,
                        PublishingDate = result.PublishingDate,
                        Id = result.Id,
                        Details = result.Details
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);// for  Debuging purpose in Client only.
            }

        }




    }
}
