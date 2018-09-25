using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Saned.Common.Comments.ComplexType;
using Saned.Common.Comments.Repository;
using Saned.Jazan.ControlPanel.Error;
using Saned.Jazan.ControlPanel.Extensions;
using Saned.Jazan.ControlPanel.Properties;
using Saned.Jazan.ControlPanel.ViewModels;
using Saned.Jazan.ControlPanel.ViewModels.Enums;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TouristVisitController : Controller
    {
        // GET: TouristVisit
        readonly ITouristVisitRepository _repo;
        private readonly ICommentRepositoryAsync _commentRepositoryAsync;


        public TouristVisitController(ITouristVisitRepository advertisementRepository)
        {
            this._repo = advertisementRepository;
            _commentRepositoryAsync = new CommentRepositoryAsync("Saned_Jazan");


        }
        // GET: Advertisement
        public ActionResult Index()
        {
            List<SelectListItem> approveList = new List<SelectListItem>
        {
              new SelectListItem { Text = "تم الموافقة", Value = "1" },
              new SelectListItem { Text = "لم يتم الموافقة ", Value = "0" }
        };
            ViewData["Approve"] = new SelectList(approveList, "Value", "Text");

            return View();
        }
        public PartialViewResult Search(TouristVisitsUserSearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
             int pageSize = Settings.Default.PageSize;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            ViewBag.keyword = search.Keyword;
            ViewBag.Page = search.Page;         
            ViewBag.IsApproved = search.IsApproved;
            var touristVisits = _repo.GetAllTouristVisits()
                .Where(u =>
                          (string.IsNullOrEmpty(search.Keyword) || u.Name.Contains(search.Keyword))
                            && (search.Approved == null || u.IsApproved == search.Approved)
                            ).OrderBy(u=>u.CreatedOn);

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


        public ActionResult Details(int id)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            TouristVisit touristVisit = _repo.GetTouristVisitById(id);
            if (touristVisit == null)
                return RedirectToAction("Index").Error("هذه الزيارة غير موجودة .");
            return View(touristVisit);
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
                    return RedirectToAction("_Comments", new { PageIndex = comment.Page-1, RelatedId= comment.RelatedId });

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

        public PartialViewResult _Comments(PagedCommentsParam comment)
        {
            ViewBag.RelatedId = comment.RelatedId;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;

            int pageSize = Settings.Default.PageSize;
            try
            {
                var pagedCommentsParam = new PagedCommentsParam()
                {
                    CommentTypeId = RelatedType.TouristVisit.GetHashCode(),
                    PageIndex = comment.PageIndex,
                    PageSize = pageSize,
                    RelatedId = comment.RelatedId
                };
                ViewBag.PageIndex = comment.PageIndex;
                ViewBag.PageSize = pageSize;
                List<CommentComplex> comments = Task.Run(() => _commentRepositoryAsync.GetPagedComments(pagedCommentsParam)).Result;

                if (comments.Count > 0)
                    ViewBag.OverallCount = comments[0].OverallCount;

               
                int result = (comments.Count() / pageSize) + (comments.Count() % pageSize > 0 ? 1 : 0);
                




                // var comments =await _commentRepositoryAsync.GetPagedComments(pagedCommentsParam);
                return PartialView("_Comments", comments);

            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return PartialView("_Comments");


            }

        }

        public PartialViewResult _GetCommentsByParentsId(int parentId)
        {
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;

            int pageSize = Settings.Default.PageSize;
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
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(TouristVisitsUserSearchModel search)
        {
            try
            {
                var touristVisit = _repo.GetTouristVisitById(search.Id);
                if (touristVisit == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                _repo.DeleteTouristVisitById(touristVisit);
                return RedirectToAction("Search", new { Page = search.Page, Keyword = search.Keyword });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return new HttpStatusCodeResult(404, "NotFound");


            }

        }


        [HttpPost]

        public ActionResult Approve(int id)
        {
            try
            {
                var touristVisit = _repo.GetTouristVisitById(id);
                if (touristVisit == null)
                    return Json(new { message = "NotFound" });
                touristVisit.IsApproved = true;
                _repo.Update(touristVisit);
                return Json(new { message = "Approved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }

        [HttpPost]

        public ActionResult UnApprove(int id)
        {
            try
            {
                var touristVisit = _repo.GetTouristVisitById(id);
                if (touristVisit == null)
                    return Json(new { message = "NotFound" });
                touristVisit.IsApproved = false;
                _repo.Update(touristVisit);
                return Json(new { message = "UnApproved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return Json(new { message = "NotUnApproved" });

            }

        }
    }

    public class DeleteCommentObj

    {

        public int Id { get; set; }
        public int Page { get; set; }
        public int OverallCount { get; set; }
        public string RelatedId { get; set; }
    }
}