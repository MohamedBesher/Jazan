using Microsoft.AspNet.Identity;
using Saned.Common.Comments.Repository;
using Saned.Common.Views.Model;
using Saned.Common.Views.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/Views")]
    public class ViewsController : BasicController
    {
        private readonly IViewsRepositoryAsync IViewsRepositoryAsync;

        public ViewsController()
        {
            IViewsRepositoryAsync = new ViewsRepositoryAsync("Saned_Jazan");
        }

        [Route("SaveView")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveView([FromBody] View View)
        {
            string userName = User.Identity.GetUserName();
            Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

            View.CreatedDate = DateTime.Now;
            if (u != null)
            {
                View.UserId = u.Id;
                View.DeviceId = null;
            }
            else
            {
                View.UserId = null;
            }

            return Ok(await IViewsRepositoryAsync.SaveView(View));
        }
    }
}
