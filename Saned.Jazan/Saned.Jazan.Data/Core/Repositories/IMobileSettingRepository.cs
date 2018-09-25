using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface IMobileSettingRepository  :IBaseRepository<MobileSetting>
    {
        Task<List<MobileSetting>> GetAll();

        Task<List<MobileSetting>> GetBySettingTypeId(int settingTypeId);


        MobileSetting GetById(int id);

        void SaveMobileSetting();
    }
}
