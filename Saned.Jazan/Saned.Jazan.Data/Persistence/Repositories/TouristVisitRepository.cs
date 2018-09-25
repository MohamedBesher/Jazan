using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class TouristVisitRepository : BaseRepository<TouristVisit>, ITouristVisitRepository
    {
        public async Task<List<TouristVisit_SelectPagedResult>> SelectPaged(int pageSize, int pageNumber, string userId, bool? isApproved)
        {
            var userIdParam = !string.IsNullOrEmpty(userId) ?
                            new SqlParameter("UserId", userId) :
                            new SqlParameter("UserId", DBNull.Value);

            var pageSizeParam = new SqlParameter("PageSize", pageSize);
            var pageNumberParam = new SqlParameter("PageNumber", pageNumber);
            SqlParameter isApprovedParam;
            if (isApproved.HasValue)
            {
                isApprovedParam = new SqlParameter("IsApproved", isApproved.Value);
            }
            else
            {
                isApprovedParam = new SqlParameter("IsApproved", DBNull.Value);
            }
            return await _context.Database.SqlQuery<TouristVisit_SelectPagedResult>("TouristVisit_SelectPaged @PageNumber , @PageSize , @UserId , @IsApproved ",
                                              pageNumberParam, pageSizeParam, userIdParam, isApprovedParam).ToListAsync();
        }

        public async Task<List<TouristVisit_SelectByIdResult>> SelectById(int id)
        {
            var idParamter = new SqlParameter("Id", id);
            var result = await _context.Database.SqlQuery<TouristVisit_SelectByIdResult>("TouristVisit_SelectById @Id", idParamter).ToListAsync();
            return result;
        }

        public async Task<Advertisement_SelectSummaryCount_Result> SelectTouristVisitSummaryCount()
        {
            var result = await _context.Database.SqlQuery<Advertisement_SelectSummaryCount_Result>
                ("TouristVisit_SelectSummaryCount").FirstOrDefaultAsync();
            return result;
        }

        public IQueryable<TouristVisit> GetAllTouristVisits()
        {
            return _context.TouristVisits.Include(u => u.CreatedByUser).AsNoTracking();
        }

        public TouristVisit GetTouristVisitById(int id)
        {
            return
                _context.TouristVisits.Include(u => u.CreatedByUser).Include(u => u.TouristVisitImages).FirstOrDefault(d => d.Id == id);
        }

        public void DeleteTouristVisitById(TouristVisit visit)
        {
            _context.TouristVisits.Remove(visit);
            _context.SaveChanges();
        }
    }
}
