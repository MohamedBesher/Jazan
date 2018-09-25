using Microsoft.AspNet.Identity;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Core.Models;
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

    [RoutePrefix("api/User")]
    public class UsersController : BasicController
    {
        private readonly AuthRepository _repo = null;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public UsersController()
        {
            _repo = new AuthRepository();
        }

        [Authorize]
        [Route("FindByUserId/{name}")]

        public async Task<IHttpActionResult> FindByUserId(string name)
        {
            var result = await _repo.FindUserByUserName(name);


            if (result != null)
            {
                var userViewModel = new UserViewModel()
                {
                    Email = result.Email,
                    FullName = result.Name,
                    PhotoUrl = result.PhotoUrl,
                    PhoneNumber = result.PhoneNumber,
                    UserId = result.Id,
                    UserName = result.UserName
                };


                return Ok(userViewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("EditUser")]

        public async Task<IHttpActionResult> EditUser(UserViewModel userViewModel)
        {
            string userName = User.Identity.GetUserName();

            var user = await _repo.FindUserByUserName(userName);

            try
            {
                if (user != null)
                {
                    user.Name = userViewModel.FullName;
                    user.PhoneNumber = userViewModel.PhoneNumber;

                    if (!string.IsNullOrEmpty(userViewModel.PhotoUrl))
                    {
                        user.PhotoUrl = userViewModel.PhotoUrl.SaveImage("Advertisement", "jpg");
                    }

                    var result = await _repo.UpdateUser(user);


                    if (result.Succeeded)
                    {
                        return Ok(new
                        {
                            user.Name,
                            user.PhoneNumber,
                            user.PhotoUrl,
                            user.Id
                        });
                    }
                    else
                    {
                        return GetErrorResult(result);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
