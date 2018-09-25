using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Owin.Security.Provider;
using PagedList;
using Saned.Jazan.ControlPanel.Error;
using Saned.Jazan.ControlPanel.Extensions;
using Saned.Jazan.ControlPanel.Properties;
using Saned.Jazan.ControlPanel.ViewModels;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        // GET: Users
        readonly IUsersRepository _repo;
        readonly ICategoryRepository _repoCategory;
        readonly IPackageRepository _repoPackage;
        readonly ITouristVisitRepository _repoTouristVisit;

        readonly IAdvertisementRepository _repoAdvertisement;

        public UsersController(IUsersRepository repo, ICategoryRepository category, IPackageRepository package, IAdvertisementRepository advertisement, ITouristVisitRepository advertisementRepository)
        {
            _repo = repo;
            this._repoCategory = category;
            this._repoPackage = package;
            this._repoAdvertisement = advertisement;
            this._repoTouristVisit = advertisementRepository;

        }
        // GET: User
        public ActionResult Index()
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            return View();
        }
        public PartialViewResult Search(UserSearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize= Settings.Default.PageSize;
            ViewBag.PageSize = pageSize;
            ViewBag.keyword = search.Keyword;
            ViewBag.Page = search.Page;

            var packages = _repo.GetAllUsers()
                .Where(u =>
                          (string.IsNullOrEmpty(search.Keyword) || u.Name.Contains(search.Keyword) || u.UserName.Contains(search.Keyword))

                            ).OrderBy(u=>u.CreatedDate);



            ViewBag.ResultCount = packages.Count();

            int result = (packages.Count() / pageSize) + (packages.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(packages.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(packages.ToPagedList(search.Page, pageSize));


        }

        
        [HttpPost]

        public ActionResult Approve(string id)
        {
            try
            {
                var user = _repo.GetUserbyId(id);
                if (user == null)
                    return Json(new { message = "NotFound" });
                user.Approve();
                _repo.Update();
                return Json(new { message = "Approved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }
        [HttpPost]
        public ActionResult UnApprove(string id)
        {
            try
            {
                var user = _repo.GetUserbyId(id);
                if (user == null)
                    return Json(new { message = "NotFound" });
                user.UnApprove();
                _repo.Update();
                return Json(new { message = "UnApproved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }
        public ActionResult Details(string id)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            ApplicationUser user=_repo.GetUserbyId(id);
            if (user==null)
                return HttpNotFound();
            return View(user);
        }

        [HttpPost]

        public ActionResult Delete(UserDeleteObj user)
        {
            try
            {
                var selected = _repo.GetUserbyId(user.Id);
                if (selected == null)
                    return new HttpStatusCodeResult(404, "NotFound");

                if (_repo.GetAdvertisementbyUserId(user.Id).Any())
                    return new HttpStatusCodeResult(404, "UnableToDelete");


                _repo.DeleteUser(selected);
                return RedirectToAction("Search", "Users", new { Page = user.Page, keyword = user.Keyword });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return new HttpStatusCodeResult(404, "NotDeleted");


            }

        }

        /// <returns> return partial view contain Search form</returns>
        public ActionResult UserAdvertisement(string id)
        {
            List<SelectListItem> approveList = new List<SelectListItem>
        {
              new SelectListItem { Text = "تم الموافقة", Value = "1" },
              new SelectListItem { Text = "لم يتم الموافقة ", Value = "0" }
        };
            ViewData["Approve"] = new SelectList(approveList, "Value", "Text");
            ViewData["Categories"] = new SelectList(_repoCategory.GetAll().ToList(), "CategoryId", "CategoryNameAr");
            ViewData["Packages"] = new SelectList(_repoPackage.GetAll().ToList(), "Id", "ArabicName");
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            ViewBag.UserId = id;
            return PartialView();
        }      
        /// <returns> return partial view contain Result of search form</returns>
        public PartialViewResult SearchAdvertisement(AdvertismentUserSearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
           int pageSize = Settings.Default.PageSize;
            ViewBag.keyword = search.Keyword;
            ViewBag.Page = search.Page;
            ViewBag.CategoryId = search.CategoryId;
            ViewBag.PackageId = search.PackageId;
            ViewBag.UserId = search.UserId;
            ViewBag.IsApproved = search.IsApproved;
            var advertisements = _repo.GetAdvertisementbyUserId(search.UserId)
                .Where(u =>
                          (string.IsNullOrEmpty(search.Keyword) || u.Name.Contains(search.Keyword))
                            && (search.CategoryId == 0 || u.CategoryId == search.CategoryId)
                            && (search.PackageId == 0 || u.PackageId == search.PackageId)
                            && (search.Approved == null || u.IsApproved == search.Approved)
                            ).OrderBy(u=>u.CreatedOn);
            ViewBag.ResultCount = advertisements.Count();
            int result = (advertisements.Count() / pageSize) + (advertisements.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page > 1 &&  result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(advertisements.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(advertisements.ToPagedList(search.Page, pageSize));
         
        }


        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteAdvertisement(AdvertismentSearchModel search)
        {



            try
            {
                var ads = _repoAdvertisement.SelectAdId(search.Id);
                if (ads == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                _repoAdvertisement.DeleteAd(search.Id);
                return RedirectToAction("SearchAdvertisement", new { Page = search.Page, Keyword = search.Keyword, CategoryId = search.CategoryId, PackageId = search.PackageId, UserId=search.UserId });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return new HttpStatusCodeResult(404, "NotFound");


            }

        }



        [HttpPost]
        public ActionResult DeleteTouristVisit(TouristVisitsUserSearchModel search)
        {
            try
            {
                var touristVisit = _repoTouristVisit.GetTouristVisitById(search.Id);
                if (touristVisit == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                _repoTouristVisit.DeleteTouristVisitById(touristVisit);
                return RedirectToAction("SearchTouristVisits", new { Page = search.Page, Keyword = search.Keyword , UserId = search.UserId });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return new HttpStatusCodeResult(404, "NotFound");


            }

        }

        [System.Web.Mvc.HttpPost]

        public ActionResult ApproveAdvertisement(int id)
        {
            try
            {
                var ads = _repoAdvertisement.SelectAdId(id);
                if (ads == null)
                    return Json(new { message = "NotFound" });
                ads.IsApproved = true;
                _repoAdvertisement.Update(ads);
                return Json(new { message = "Approved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }

        [System.Web.Mvc.HttpPost]

        public ActionResult UnApproveAdvertisement(int id)
        {
            try
            {
                var ads = _repoAdvertisement.SelectAdId(id);
                if (ads == null)
                    return Json(new { message = "NotFound" });
                ads.IsApproved = false;
                _repoAdvertisement.Update(ads);
                return Json(new { message = "UnApproved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return Json(new { message = "NotUnApproved" });

            }

        }

        public ActionResult UserTouristVisits(string id)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            ViewBag.UserId = id;
            return PartialView();
        }
        public PartialViewResult SearchTouristVisits(TouristVisitsUserSearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize = Settings.Default.PageSize;
            ViewBag.keyword = search.Keyword;
            ViewBag.Page = search.Page;        
            ViewBag.UserId = search.UserId;
            ViewBag.CityName = search.CityName;
            var touristVisits = _repo.GetTouristVisitsbyUserId(search.UserId)
                .Where(u =>
                          (string.IsNullOrEmpty(search.Keyword) || u.Name.Contains(search.Keyword))
                            && (string.IsNullOrEmpty(search.CityName) || u.CityName == search.CityName)
                            ).OrderBy(u => u.CreatedOn);
            ViewBag.ResultCount = touristVisits.Count();

            int result = (touristVisits.Count() / pageSize) + (touristVisits.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page>1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(touristVisits.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(touristVisits.ToPagedList(search.Page, pageSize));
        }


        [HttpPost]

        public ActionResult ApproveTouristVisit(int id)
        {
            try
            {
                var touristVisit = _repoTouristVisit.GetTouristVisitById(id);
                if (touristVisit == null)
                    return Json(new { message = "NotFound" });
                touristVisit.IsApproved = true;
                _repoTouristVisit.Update(touristVisit);
                return Json(new { message = "Approved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }

        [HttpPost]

        public ActionResult UnApproveTouristVisit(int id)
        {
            try
            {
                var touristVisit = _repoTouristVisit.GetTouristVisitById(id);
                if (touristVisit == null)
                    return Json(new { message = "NotFound" });
                touristVisit.IsApproved = false;
                _repoTouristVisit.Update(touristVisit);
                return Json(new { message = "UnApproved" });
            }
            catch (Exception e)
            {

                ErrorSaver.SaveError(e);
                return Json(new { message = "NotUnApproved" });

            }

        }


    }

    public class UserDeleteObj:Pager
    {
        public string Id { get; set; }
        public string Keyword { get; set; }
    }
}