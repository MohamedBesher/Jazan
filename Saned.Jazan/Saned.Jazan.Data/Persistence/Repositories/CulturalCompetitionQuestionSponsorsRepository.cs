using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System.Threading.Tasks;
using Saned.Jazan.Data.Core.Dtos;
using System;
using System.Collections.Generic;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class CulturalCompetitionQuestionSponsorsRepository : BaseRepository<CulturalCompetitionQuestionSponsor>,
        ICulturalCompetitionQuestionSponsorsRepository
    {
        public async Task<List<CulturalCompetitionQuestionSponsorsDto>> SelectActiveSponsors()
        {
            var result = await _context.Database.SqlQuery<CulturalCompetitionQuestionSponsorsDto>("CulturalCompetitionQuestionSponsors_SelectSponsors").ToListAsync();
            return result;
        }
    }
}
