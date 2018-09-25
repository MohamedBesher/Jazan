using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System.Threading.Tasks;
using System.Xml.Linq;
using Saned.Jazan.Data.Core.Dtos;
using Z.EntityFramework.Plus;

namespace Saned.Jazan.Data.Persistence.Repositories
{
    public class CulturalCompetitionQuestionRepository : BaseRepository<CulturalCompetitionQuestion>, ICulturalCompetitionQuestionRepository
    {
        public async Task<CulturalCompetitionQuestionDto> SelectPublishedCulturalCompetitionQuestion()
        {
            var result = await _context.Database.SqlQuery<CulturalCompetitionQuestionDto>("CulturalCompetitionQuestion_SelectActiveQuestion").FirstOrDefaultAsync();
            return result;
        }

        public IQueryable<CulturalCompetitionQuestion> GetAll()
        {
          return  _context.CulturalCompetitionQuestions.AsNoTracking();
        }

        public CulturalCompetitionQuestion GetById(int id)
        {
            return _context.CulturalCompetitionQuestions.FirstOrDefault(u=>u.Id==id);
        }

        public bool Add(CulturalCompetitionQuestion question)
        {
            try
            {
            _context.CulturalCompetitionQuestions.Add(question);
            _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
          
        }

        public bool UpdateCulturalCompetitionQuestion(CulturalCompetitionQuestion question)
        {
            try
            {
             var culturalCompetitionQuestion = _context.CulturalCompetitionQuestions.Include(u=>u.CulturalCompetitionQuestionSponsors)
                .FirstOrDefault(x=>x.Id== question.Id);

                

                if (culturalCompetitionQuestion != null)
            {

                    _context.CulturalCompetitionQuestionSponsors.RemoveRange(
                    culturalCompetitionQuestion.CulturalCompetitionQuestionSponsors);


                    culturalCompetitionQuestion.Question = question.Question;
                 culturalCompetitionQuestion.Title = question.Title;
                 culturalCompetitionQuestion.UpdatedOn = question.UpdatedOn;
                 culturalCompetitionQuestion.IsPublished = question.IsPublished;
                 culturalCompetitionQuestion.UpdatedBy = question.UpdatedBy;
                    //_context.CulturalCompetitionQuestionSponsors.Where(u=>u.CulturalCompetitionQuestionId==question.Id).
                        
                        
                       
                culturalCompetitionQuestion.CulturalCompetitionQuestionSponsors =
                    question.CulturalCompetitionQuestionSponsors;
                _context.SaveChanges();
                return true;
            }
                return false;
            }
            catch (Exception )
            {
                return false;
            }
            

           
        }

        public bool MarkAllAsUnPublished()
        {
            try
            {
                _context.CulturalCompetitionQuestions.ToList().ForEach(u => u.IsPublished = false);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;

            }

        }

        public IQueryable<CulturalCompetitionAnswer> GetUsersByQuestionId(int id)
        {
            return _context.CulturalCompetitionAnswers.Include(u => u.CreatedByUser).Where(u=>u.CulturalCompetitionQuestionId==id);

        }

        public CulturalCompetitionAnswer GetAnswerById(int id)
        {
            return _context.CulturalCompetitionAnswers.FirstOrDefault(u=>u.Id==id);

        }

        public bool UpdateAnswer(CulturalCompetitionAnswer answer)
        {
            try
            {
               
                if (answer != null)
                {
                    answer.IsWinner = answer.IsWinner;                
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAnswer(CulturalCompetitionAnswer answer)
        {
            try
            {
                 _context.CulturalCompetitionAnswers.Remove(answer);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                
                return false;
            }           
        }

        public bool DeleteQuestion(CulturalCompetitionQuestion question)
        {
            try
            {
                _context.CulturalCompetitionQuestions.Remove(question);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public IQueryable<Sponsor> GetSponsorsByAdsId(int id)
        {
            return _context.CulturalCompetitionQuestionSponsors
                .Include(u => u.Advertisement)
                .Include(u => u.Advertisement.Category)
                .Where(u => u.CulturalCompetitionQuestionId == id).Select(u => new Sponsor()
                {
                    Value = u.AdvertisementId,
                    Text = u.Advertisement.Name,
                    PhotoUrl = u.Advertisement.ImageUrl,
                    CategoryName = u.Advertisement.Category.CategoryNameAr
                });
                ;
        }

        public int UpdateQuestion(CulturalCompetitionQuestion question)
        {
            try
            {
                _context.SaveChanges();
                return 1;
            }
            catch (Exception )
            {
                return 0;
            }
        }
    }
}
