using System.Collections.Generic;
using System.Linq;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Repositories
{
    public interface ICulturalCompetitionQuestionRepository : IBaseRepository<CulturalCompetitionQuestion>
    {
        Task<CulturalCompetitionQuestionDto> SelectPublishedCulturalCompetitionQuestion();

        IQueryable<CulturalCompetitionQuestion> GetAll();
        CulturalCompetitionQuestion GetById(int id);
        bool Add(CulturalCompetitionQuestion question);
        bool UpdateCulturalCompetitionQuestion(CulturalCompetitionQuestion question);

        bool MarkAllAsUnPublished();
        IQueryable<CulturalCompetitionAnswer> GetUsersByQuestionId(int id);


        CulturalCompetitionAnswer GetAnswerById(int id);
        bool UpdateAnswer(CulturalCompetitionAnswer answer);
        bool DeleteAnswer(CulturalCompetitionAnswer question);
        bool DeleteQuestion(CulturalCompetitionQuestion question);
        IQueryable<Sponsor> GetSponsorsByAdsId(int id);
        int UpdateQuestion(CulturalCompetitionQuestion question);
    }

    public class Sponsor
    {
        public string Text { get; set; }
        public int  Value { get; set; }
        public string PhotoUrl { get; set; }
        public string CategoryName { get; set; }
       
    }
}
