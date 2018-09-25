using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface IUsersRepository : IBaseRepository<ApplicationUser> , IDisposable
    {
        IQueryable<ApplicationUser> GetAllUsers();
        ApplicationUser GetUserbyId(string id);
        void Update();
        IQueryable<Advertisement> GetAdvertisementbyUserId(string id);
        IQueryable<TouristVisit> GetTouristVisitsbyUserId(string id);

        void DeleteUser(ApplicationUser selected);
    }
}
