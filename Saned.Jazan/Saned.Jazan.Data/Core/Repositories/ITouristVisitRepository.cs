using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface ITouristVisitRepository : IBaseRepository<TouristVisit>
    {
        Task<List<TouristVisit_SelectPagedResult>> SelectPaged(int pageSize, int pageNumber, string userId , bool? isApproved);
        Task<List<TouristVisit_SelectByIdResult>> SelectById(int id);
        Task<Advertisement_SelectSummaryCount_Result> SelectTouristVisitSummaryCount();

        IQueryable<TouristVisit> GetAllTouristVisits();
        TouristVisit GetTouristVisitById(int id);
        void DeleteTouristVisitById(TouristVisit visit);


    }
}
