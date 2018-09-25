using Microsoft.AspNet.Identity;
using Saned.Jazan.Api.Controllers;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    public static class Helper
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(
    this TSource source,
    Func<TSource, TSource> nextItem,
    Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string GetaAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }
    }

    public class BasicController : ApiController
    {
        public IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


      

        public async Task<Saned.Jazan.Data.Core.Models.ApplicationUser> GetApplicationUser(string userName)
        {
            using (var _repo = new AuthRepository())
            {
                return await _repo.FindUserByUserName(userName);
            }
          
        }

        //public static string GetaAllMessages(this Exception exception)
        //{
        //    var messages = exception.FromHierarchy(ex => ex.InnerException)
        //        .Select(ex => ex.Message);
        //    return String.Join(Environment.NewLine, messages);
        //}

        //public static IEnumerable<TSource> FromHierarchy<TSource>(
        //   this TSource source,
        //   Func<TSource, TSource> nextItem)
        //   where TSource : class
        //{
        //    return FromHierarchy(source, nextItem, s => s != null);
        //}

    }
}