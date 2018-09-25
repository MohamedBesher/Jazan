using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Dtos;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class CulturalCompetitionAnswersRepository : BaseRepository<CulturalCompetitionAnswer>, ICulturalCompetitionAnswersRepository
    {
        public async Task<List<WinnerUserDto>> SelectWinnerUsers()
        {
            var result = await _context.Database.SqlQuery<WinnerUserDto>("CulturalCompetitionAnswers_SelectWinnerUsers").ToListAsync();
            return result;
        }
    }
}
