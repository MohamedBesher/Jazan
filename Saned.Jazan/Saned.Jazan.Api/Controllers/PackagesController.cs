using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Saned.Jazan.Api.Controllers
{
    public class PackagesController : BasicController
    {
        IPackageRepository IPackageRepository;
        public PackagesController(IPackageRepository IPackageRepository)
        {
            this.IPackageRepository = IPackageRepository;
        }

        //// GET: api/Advertisements
        public async Task<IHttpActionResult> GetPackages()
        {
            return Ok(await IPackageRepository.SelectAllPackages());
        }
        
        [Route("api/Packages/")]
        // GET: api/Packages
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await IPackageRepository.SelectAllPackages());
        }

        // GET: api/Packages/5
        [ResponseType(typeof(Package))]
        public async Task<IHttpActionResult> Get(int id)
        {
            var advertisement = await IPackageRepository.SelectById(id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return Ok(advertisement);
        }
    }
}
