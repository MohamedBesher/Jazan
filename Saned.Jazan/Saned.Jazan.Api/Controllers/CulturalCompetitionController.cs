using Microsoft.AspNet.Identity;
using Saned.Jazan.Api.Models;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Saned.Jazan.Api.Controllers
{
    [RoutePrefix("api/CulturalCompetition")]
    public class CulturalCompetitionController : BasicController
    {
        private ICulturalCompetitionQuestionRepository ICulturalCompetitionQuestionRepository;
        private ICulturalCompetitionAnswersRepository ICulturalCompetitionAnswersRepository;
        private ICulturalCompetitionQuestionSponsorsRepository ICulturalCompetitionQuestionSponsorsRepository;
        public CulturalCompetitionController
            (
            ICulturalCompetitionQuestionRepository ICulturalCompetitionQuestionRepository,
            ICulturalCompetitionAnswersRepository ICulturalCompetitionAnswersRepository,
            ICulturalCompetitionQuestionSponsorsRepository ICulturalCompetitionQuestionSponsorsRepository
            )
        {
            this.ICulturalCompetitionQuestionRepository = ICulturalCompetitionQuestionRepository;
            this.ICulturalCompetitionAnswersRepository = ICulturalCompetitionAnswersRepository;
            this.ICulturalCompetitionQuestionSponsorsRepository = ICulturalCompetitionQuestionSponsorsRepository;
        }

        [Route("SelectActiveQuestion")]
        [HttpGet]
        public async Task<IHttpActionResult> SelectActiveQuestion()
        {
            var result = await ICulturalCompetitionQuestionRepository.SelectPublishedCulturalCompetitionQuestion();
            return Ok(result);
        }

        [Route("SelectWinnerUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> SelectWinnerUsers()
        {
            var result = await ICulturalCompetitionAnswersRepository.SelectWinnerUsers();
            return Ok(result);
        }

        [Route("SelectActiveSponsors")]
        [HttpGet]
        public async Task<IHttpActionResult> SelectActiveSponsors()
        {
            var result = await ICulturalCompetitionQuestionSponsorsRepository.SelectActiveSponsors();
            return Ok(result);
        }

        [Route("SaveAnswer")]
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> SaveAnswer(CulturalCompetitionAnswerViewModel culturalCompetitionAnswerViewModel)
        {
            string userName = User.Identity.GetUserName();
            Data.Core.Models.ApplicationUser u = await GetApplicationUser(userName);

            var result = await ICulturalCompetitionAnswersRepository.CreateAsync(new CulturalCompetitionAnswer()
            {
                CreatedBy = u.Id,
                CreatedOn = DateTime.Now,
                CulturalCompetitionQuestionId = culturalCompetitionAnswerViewModel.QuestionId,
                Value = culturalCompetitionAnswerViewModel.Value,
                IsWinner = false
            });

            return Ok(result);
        }
    }
}
