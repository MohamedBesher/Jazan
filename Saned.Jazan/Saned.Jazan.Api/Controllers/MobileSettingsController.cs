using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    public class MobileSettingsController : ApiController
    {
        IMobileSettingRepository IMobileSettingRepository;
        public MobileSettingsController(IMobileSettingRepository IMobileSettingRepository)
        {
            this.IMobileSettingRepository = IMobileSettingRepository;
        }

        [HttpGet]
        [Route("api/MobileSettings/GetAllMobileSetting")]
        public async Task<IHttpActionResult> GetAllMobileSetting()
        {
            var result = await IMobileSettingRepository.GetAll();

            return Ok((from r in result
                       select new MobileSettingDto()
                       {
                           Id = r.Id,
                           SettingTypeId = (int)r.SettingType,
                           SettingTypeName = r.SettingType.ToString(),
                           Value = r.Value
                       }));

        }

        [HttpGet]
        [Route("api/MobileSettings/GetMobileSettingBySettingType/{settingTypeId?}")]
        public async Task<IHttpActionResult> GetMobileSettingBySettingType(int settingTypeId)
        {
            var result = await IMobileSettingRepository.GetBySettingTypeId(settingTypeId);

            return Ok((from r in result
                       select new MobileSettingDto()
                       {
                           Id = r.Id,
                           SettingTypeId = (int)r.SettingType,
                           SettingTypeName = r.SettingType.ToString(),
                           Value = r.Value
                       }));

        }
    }
}
