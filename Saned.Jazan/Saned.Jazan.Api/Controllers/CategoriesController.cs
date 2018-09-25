using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/Categories")]

    public class CategoriesController : ApiController
    {
        ICategoryRepository repo;
        public CategoriesController(ICategoryRepository repo)
        {
            this.repo = repo;
        }


        [HttpPost]
        [Route("GetCategories/{ParentId?}")]
        public IHttpActionResult GetCategories([FromUri]int? ParentId, [FromBody] CategoriesRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (ParentId == 0)
                {
                    var allParentIds = repo.All().Select(x => x.ParentId);
                    var result = repo.All().Where(x => x.ParentId == 0 && allParentIds.Contains(x.CategoryId))
                        .Select(x => new CategoryDtos()
                        {
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryNameAr,
                            ImageUrl = x.CategoryImage

                        }).ToList();
                    return Ok(result);
                }
                else
                {
                    var result = repo.All().Where(x => x.ParentId == ParentId)
                        .Select(x => new CategoryDtos()
                        {
                            CategoryId = x.CategoryId,
                            CategoryName = x.CategoryNameAr,
                            ImageUrl = x.CategoryImage

                        }).ToList();
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



    }
}
