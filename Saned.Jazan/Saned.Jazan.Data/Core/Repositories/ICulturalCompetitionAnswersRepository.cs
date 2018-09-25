using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface ICulturalCompetitionAnswersRepository : IBaseRepository<CulturalCompetitionAnswer>
    {
        Task<List<WinnerUserDto>> SelectWinnerUsers();
    }
}
