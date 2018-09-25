using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;
using Saned.Common.Comments.ComplexType;
using Saned.Common.Comments.Repository;
using Saned.Common.Notification.Model;
using Saned.Jazan.ControlPanel.Extensions;
using Saned.Jazan.ControlPanel.Properties;
using Saned.Jazan.ControlPanel.ViewModels;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Common.Notification.Repository;
using Saned.Jazan.ControlPanel.Error;
using Saned.Jazan.ControlPanel.ViewModels.Enums;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdvertisementController : Controller
    {
        readonly IAdvertisementRepository _repo;
        readonly ICategoryRepository _repoCategory;
        readonly IUsersRepository _repoUser;
        readonly IPackageRepository _repoPackage;
        private INotificationsRepositoryAsync _notificationsRepositoryAsync;
        private readonly ICommentRepositoryAsync _commentRepositoryAsync;
        readonly OneSignalLibrary.OneSignalClient _client;
        private readonly int _notificationFeatureId= Settings.Default.NotificationFeatureId;

        public AdvertisementController(IAdvertisementRepository advertisementRepository,ICategoryRepository category, IPackageRepository package, IUsersRepository repoUser)
        {
            this._repo = advertisementRepository;
            this._repoCategory = category;
            this._repoPackage = package;
            this._repoUser = repoUser;
            _commentRepositoryAsync = new CommentRepositoryAsync("Saned_Jazan");

            _client = new OneSignalLibrary.OneSignalClient(Settings.Default.appKey,
                                                        Settings.Default.resetKey,
                                                         Settings.Default.userAuth);
        }
        // GET: Advertisement
        public ActionResult Index()
        {
            List < SelectListItem > approveList = new List<SelectListItem>
        {
              new SelectListItem { Text = "تم الموافقة", Value = "1" },
              new SelectListItem { Text = "لم يتم الموافقة ", Value = "0" }
        };
            ViewData["Approve"] = new SelectList(approveList, "Value", "Text");
            ViewData["Categories"] = new SelectList(_repoCategory.GetAllSub().ToList(), "CategoryId", "CategoryNameAr");
            ViewData["Packages"] = new SelectList(_repoPackage.GetAll().ToList(), "Id", "ArabicName");

            return View();
            //var viewModel = new AdvertisementViewModel()
            //{
            //    Advertisements = _repo.GetAll()
            //};
            //return View(viewModel.Advertisements.ToPagedList(page, 5));
           // return View();


        }
        public PartialViewResult Search(AdvertismentSearchModel  search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;         
            int pageSize = Settings.Default.PageSize;         
            ViewBag.keyword = search.Keyword;
            ViewBag.Page = search.Page;
            ViewBag.CategoryId = search.CategoryId;
            ViewBag.PackageId = search.PackageId;
            ViewBag.IsApproved = search.IsApproved;
            var advertisements = _repo.GetAll(_notificationFeatureId)
                .Where(u => 
                          (string.IsNullOrEmpty(search.Keyword) || u.Name.Contains(search.Keyword))
                            && (search.CategoryId == 0 || u.CategoryId == search.CategoryId)
                            && (search.PackageId == 0 || u.PackageId == search.PackageId)
                            && (search.Approved == null || u.IsApproved == search.Approved)
                            ).OrderByDescending(u=>u.CreatedOn);
           
            ViewBag.ResultCount = advertisements.Count();
            int result = (advertisements.Count() / pageSize)+ (advertisements.Count()% pageSize >0 ? 1 :0) ;
            if (search.Page>1 && result < search.Page)
            {
             ViewBag.Page = search.Page-1;
            return PartialView(advertisements.ToPagedList(search.Page - 1,pageSize));
            }
            else
                return PartialView(advertisements.ToPagedList(search.Page,pageSize));
           
            //return PartialView(data);
        }

        public PartialViewResult GetSentNotificationCount(int id)
        {
            int count = _repo.GetSentNotificationCountById(id);
            ViewBag.NotificationCount = count;
            return PartialView("_GetSentNotificationCount", count);


        }



        public ActionResult Details(int id)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            Advertisement  advertisement= _repo.SelectAdIdwithImages(id);
            return View(advertisement);
        }


        public ActionResult _Details(int id)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            Advertisement advertisement = _repo.SelectAdIdwithImages(id);
            return PartialView("_Details",advertisement);
        }


        //[System.Web.Http.Route("GetPagedComments/{pageIndex}/{pageSize}/{relatedId?}/{commentTypeId?}")]
        //[HttpPost]
        public PartialViewResult _Comments(PagedCommentsParam comment)
        {
            ViewBag.RelatedId = comment.RelatedId;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;

            int pageSize = Settings.Default.PageSize;
            try
            {
                var pagedCommentsParam = new PagedCommentsParam()
                {
                    CommentTypeId = RelatedType.Advertisment.GetHashCode(),
                    PageIndex = comment.PageIndex,
                    PageSize = pageSize,
                    RelatedId = comment.RelatedId,
                
                    
                };
                ViewBag.PageIndex = comment.PageIndex;
                ViewBag.PageSize = pageSize;
                List<CommentComplex> comments = Task.Run(() => _commentRepositoryAsync.GetPagedComments(pagedCommentsParam)).Result;
               

               // var comments =await _commentRepositoryAsync.GetPagedComments(pagedCommentsParam);
                return PartialView("_Comments", comments);

            }
            catch (Exception )
            {

                return PartialView("_Comments");


            }

        }

        public PartialViewResult _GetCommentsByParentsId(int parentId)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;

            //int pageSize = Settings.Default.PageSize;
            try
            {

                List<CommentComplex> comments = Task.Run(() => _commentRepositoryAsync.GetCommentByParentId(parentId)).Result;


                return PartialView("_GetCommentsByParentsId", comments);

            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return PartialView("_GetCommentsByParentsId");


            }

        }

        [HttpPost]
        public ActionResult DeleteComment(DeleteCommentObj comment)
        {
            try
            {
                int pageSize = Settings.Default.PageSize;

                Task.Run(() => _commentRepositoryAsync.DeleteComment(comment.Id));


                int resultCount = comment.OverallCount;
                int result = (resultCount / pageSize) + (resultCount % pageSize > 0 ? 1 : 0);
                if (result < comment.Page)
                {
                    return RedirectToAction("_Comments", new { PageIndex = comment.Page - 1, RelatedId = comment.RelatedId });

                }
                else
                    return RedirectToAction("_Comments", new { PageIndex = comment.Page, RelatedId = comment.RelatedId });


            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return new HttpStatusCodeResult(404, "NotFound");


            }

        }

        public ActionResult Add()
        {          
            return View();
        }
        public ActionResult Edit(int id)
        {

            ViewData["Categories"] = new SelectList(_repoCategory.GetAllSub().ToList(), "CategoryId", "CategoryNameAr");
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            ViewBag.ApiUrl = Settings.Default.ApiUrl;
            ViewBag.BackEndUrl = Settings.Default.BackEndUrl;
            Advertisement advertisement = _repo.SelectAdIdwithImages(id);
            return View(advertisement);
            
        }
        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(AdvertismentSearchModel search)
        {



            try
            {
             var ads = _repo.SelectAdId(search.Id);
            if (ads == null)
                return new HttpStatusCodeResult(404, "NotFound");             
                _repo.DeleteAd(search.Id);
             return RedirectToAction("Search", new {Page = search.Page,Keyword=search.Keyword, CategoryId = search.CategoryId, PackageId = search.PackageId });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return new HttpStatusCodeResult(404, "NotFound");


            }

        }


        [System.Web.Mvc.HttpPost]

        public ActionResult Approve(int id)
        {
            try
            {
                var ads = _repo.SelectAdIdwithImages(id);
                if (ads == null)
                    return Json(new { message = "NotFound" });
                ads.IsApproved = true;
                int period = ads.Package.Period;
                ads.StartDate =DateTime.Now;
                ads.EndDate =DateTime.Now.AddDays(period);
                _repo.Update(ads);
                return Json(new { message = "Approved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }

        [System.Web.Mvc.HttpPost]

        public ActionResult UnApprove(int id)
        {
            try
            {
                var ads = _repo.SelectAdId(id);
                if (ads == null)
                    return Json(new { message = "NotFound" });
                ads.IsApproved = false;
                _repo.Update(ads);
                return Json(new { message = "UnApproved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return Json(new { message = "NotUnApproved" });

            }

        }


        public ActionResult Notify(int id)
        {


            var ads = _repo.GetAll(_notificationFeatureId).FirstOrDefault(u => u.Id == id);
            if (ads == null)
                return RedirectToAction("Index","Advertisement").Error("هذا الاعلان غير موجود.");


            if (!ads.IsApproved)
                return RedirectToAction("Index", "Advertisement").Error("لا يمكن ارسال تنبيهات لهذا اعلان غير مفعل .");


            int? quantity = ads.AdvertisementFeatures.FirstOrDefault()?.Quantity;
            if (quantity == null)
                return RedirectToAction("Index", "Advertisement").Error("لا يمكن ارسال تنبيهات لهذا الاعلان.");

            //check notification count

            int count = _repo.GetSentNotificationCountById(id);
            if (quantity - count <= 0)
                return RedirectToAction("Index", "Advertisement").Error("لقد تجاوزت الحد المسموح من التنبيهات");


            ViewBag.AdvertisementId = id;
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public  ActionResult Notify(NotificationViewModel notification)
        {
            try
            {


                if (ModelState.IsValid)
                {

                    var ads = _repo.GetAll(_notificationFeatureId).FirstOrDefault(u => u.Id == notification.Id);
                    if (ads == null)
                        return RedirectToAction("Index").Error("هذا الاعلان غير موجود.");


                    int? quantity = ads.AdvertisementFeatures.FirstOrDefault()?.Quantity;
                    if (quantity == null)
                        return RedirectToAction("Index").Error("لا يمكن ارسال تنبيهات لهذا الاعلان");


                   // check notification count

                    int count = _repo.GetSentNotificationCountById(notification.Id);
                    if (quantity - count <= 0)
                        return RedirectToAction("Index").Error("لقد تجاوزت الحد المسموح من التنبيهات");


                    _notificationsRepositoryAsync = new NotificationRepositoryAsync("Saned_Jazan");
                    Task.Run(
                        () =>
                            SendNotification(
                                new Notifications()
                                {
                                    ArabicMessage = notification.Content,
                                    RelatedId = notification.Id.ToString(),
                                    RelatedType = RelatedType.Advertisment.GetHashCode().ToString(),
                                    EnglishMessage = notification.Content
                                }, ads.UserId));
                 
                    return RedirectToAction("Index").Success("تم ارسال التنبيه بنجاح .");
                }
                return View("Notify", notification);
            }
            catch (Exception e)

            {
                ErrorSaver.SaveError(e);              
                return RedirectToAction("Index").Error("حدث خطأ أثناء ارسال التنبيه .");


            }


        }

        public async Task<bool> SendNotification(Notifications Notification,string userId)
        {
            try
            {
               //need to include only user that is approved is true


                var devices = await _notificationsRepositoryAsync.GetDevices();
                var devicesList = devices
                    .Where(u=>u.UserId!=userId && !string.IsNullOrEmpty(u.DeviceId)  && u.DeviceId!="null")                 
                    .GroupBy(test => test.DeviceId)
                   .Select(grp => grp.First())
                   .Select(u=>u.DeviceId);
                  
                var recepientDevices = new OneSignalLibrary.Posting.Device(new HashSet<string>(devicesList));
                Dictionary<string, string> notificationContent = new Dictionary<string, string>();
                notificationContent.Add("ar", Notification.ArabicMessage);
                notificationContent.Add("en", Notification.EnglishMessage);
                var content = new OneSignalLibrary.Posting.ContentAndLanguage(notificationContent);
                Notification.NotificationLogs =
                                (from device in devices
                                 select new NotificationLog()
                                 {
                                     isSeen = false,
                                     RecieverId = device.UserId
                                 }).GroupBy(n => new { n.RecieverId })
                                           .Select(g => g.FirstOrDefault()).ToList();

                 await _notificationsRepositoryAsync.AddNotification(Notification);             
                try
                {
                    _client.SendNotification(recepientDevices, content, null, null);
                    return true;
                }
                catch (Exception ex)
                {
                    ErrorSaver.SaveError(ex);
                    return false;
                }

               

            }
            catch (Exception ex)
            {
                ErrorSaver.SaveError(ex);
                return false;
            }
        }
    }
}