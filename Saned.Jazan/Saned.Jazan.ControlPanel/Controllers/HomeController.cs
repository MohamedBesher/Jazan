using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Saned.Common.Comments.Repository;
using Saned.Common.Notification.Repository;
using Saned.Jazan.ControlPanel.Properties;
using Saned.Jazan.Data.Core.Repositories;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        readonly IAdvertisementRepository _repo;
        readonly ICategoryRepository _repoCategory;
        readonly IUsersRepository _repoUser;
        readonly IPackageRepository _repoPackage;
        readonly ITouristVisitRepository _repoTourist;
        readonly ICulturalCompetitionQuestionRepository _repoCompetitionQuestion;


        readonly OneSignalLibrary.OneSignalClient _client;
        private readonly int _notificationFeatureId = Settings.Default.NotificationFeatureId;

        public HomeController(IAdvertisementRepository advertisementRepository, ICategoryRepository category, IPackageRepository package, IUsersRepository repoUser, ITouristVisitRepository repoTourist, ICulturalCompetitionQuestionRepository question)
        {
            this._repo = advertisementRepository;
            this._repoCategory = category;
            this._repoPackage = package;
            this._repoUser = repoUser;
            this._repoTourist = repoTourist;
            this._repoCompetitionQuestion = question;


            _client = new OneSignalLibrary.OneSignalClient(Settings.Default.appKey,
                                                        Settings.Default.resetKey,
                                                         Settings.Default.userAuth);
        }
        // GET: Advertisement

        public ActionResult Index()
        {
            int approvedAdvertisements=_repo.GetAll().Count(u => u.IsApproved);
            int unapprovedAdvertisements=_repo.GetAll().Count(u => !u.IsApproved);

            int approvedUsers= _repoUser.GetAllUsers().Count(u => u.IsApprove.Value);
            int unapprovedUsers= _repoUser.GetAllUsers().Count(u => !u.IsApprove.Value);

            int approvedTouristVisits = _repoTourist.GetAllTouristVisits().Count(u => u.IsApproved);
            int unapprovedTouristVisits = _repoTourist.GetAllTouristVisits().Count(u => !u.IsApproved);

            int approvedCompetitionQuestion = _repoCompetitionQuestion.GetAll().Count(u => u.IsPublished);
            int unapprovedCompetitionQuestion = _repoCompetitionQuestion.GetAll().Count(u => !u.IsPublished);

            ViewBag.approvedAdvertisements = approvedAdvertisements;
            ViewBag.unapprovedAdvertisements = unapprovedAdvertisements;

            ViewBag.approvedUsers = approvedUsers;
            ViewBag.unapprovedUsers = unapprovedUsers;

            ViewBag.approvedTouristVisits = approvedTouristVisits;
            ViewBag.unapprovedTouristVisits = unapprovedTouristVisits;

            ViewBag.approvedCompetitionQuestion = approvedCompetitionQuestion;
            ViewBag.unapprovedCompetitionQuestion = unapprovedCompetitionQuestion;






            //return RedirectToAction("Index", "Advertisement");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}