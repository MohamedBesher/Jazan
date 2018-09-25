using Microsoft.AspNet.Identity;
using Saned.Common.Rating.ComplexType;
using Saned.Common.Rating.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/Rating")]
    public class RatingController : BasicController
    {
        private readonly IRatingElementRepositoryAsync IRatingElementRepositoryAsync;
        public RatingController()
        {
            IRatingElementRepositoryAsync = new RatingElementRepositoryAsync("Saned_Jazan");
        }
        [Route("GetRatings")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllRatings()
        {
            try
            {
                return Ok(await IRatingElementRepositoryAsync.GetAll());
            }
            catch (Exception e)
            {

                string msg = e.GetaAllMessages();
                return BadRequest("GetRatings --- " + msg);
            }

        }

        [Route("SaveRating")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SaveRating([FromBody] RatingInfo rating)
        {
            try
            {
                string userName = User.Identity.GetUserName();
                Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

                rating.RatingDate = DateTime.Now;
                rating.UserId = u.Id;
                if (string.IsNullOrEmpty(rating.Description))
                {
                    rating.Description = string.Empty;
                }

                return Ok(await IRatingElementRepositoryAsync.SaveRating(rating));
            }
            catch (Exception e)
            {

                string msg = e.GetaAllMessages();
                return BadRequest("SaveRating --- " + msg);
            }
        }
    }
}