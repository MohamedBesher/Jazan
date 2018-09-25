using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System.Data.SqlClient;
using System.Linq;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class MobileSettingRepository : BaseRepository<MobileSetting>, IMobileSettingRepository
    {
        public async Task<List<MobileSetting>> GetAll()
        {
            var result = await _context.Database.SqlQuery<MobileSetting>("MobileSetting_SelectAll").ToListAsync();
            return result;
        }

        public async Task<List<MobileSetting>> GetBySettingTypeId(int settingTypeId)
        {
            var settingTypeParam = new SqlParameter("SettingType", settingTypeId);
            var result = await _context.Database.SqlQuery<MobileSetting>("MobileSetting_SelectBySettingType @SettingType",settingTypeParam).ToListAsync();
            return result;
        }

        public MobileSetting GetById(int id)
        {
           return _context.MobileSettings.FirstOrDefault(u => u.Id == id);
        }


        public void SaveMobileSetting()
        {
             _context.SaveChanges();
        }


    }
}
