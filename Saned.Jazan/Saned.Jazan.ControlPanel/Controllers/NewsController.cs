using System;
using System.Linq;


using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Saned.Jazan.ControlPanel.Extensions;

using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using PagedList;
using Saned.Jazan.ControlPanel.Error;
using Saned.Jazan.ControlPanel.Properties;
using Saned.Jazan.ControlPanel.ViewModels;
using static System.Data.Entity.Core.Objects.EntityFunctions;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class NewsController : Controller
    {
        readonly INewsRepository _repo;
        public NewsController(INewsRepository newsRepository)
        {
            this._repo = newsRepository;
        }





        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Search(NewsSearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize = Settings.Default.PageSize;
            ViewBag.Page = search.Page;
            ViewBag.SearchTerm = search.SearchTerm;
           
            ViewBag.PublishingDate = search.PublishingDate;
                   


            var news = _repo.GetAll()
                     .Where(u => 
                     (string.IsNullOrEmpty(search.SearchTerm)  || u.Title.Contains(search.SearchTerm))
                           && (!search.PublishingDate.HasValue || TruncateTime(u.PublishingDate) == TruncateTime(search.PublishingDate)))
                           .OrderByDescending(u=>u.PublishingDate);

            ViewBag.ResultCount = news.Count();

            int result = (news.Count() / pageSize) + (news.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(news.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(news.ToPagedList(search.Page, pageSize));



        }




        public ActionResult AddNews()
        {
           

            return View("AddNews",new NewsFormViewModel());
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddNews(NewsFormViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View("AddNews", model);
            }

            var news = new News
            {
                Title = model.Title,
                Details = model.Details,
                PublishingDate = DateTime.Now
            };
            if (!string.IsNullOrEmpty(model.Image))
            {
                var img = new NewsImage()
                {
                    ImagePath = model.Image,
                    IsDefault = true,
                    NewsId = news.Id

                };
                news.NewsImages.Add(img);
            }
           
           

            _repo.SaveNews(news);



            var message = string.Format("تم إضافة الخبر  {0} بنجاح", news.Title);

            return RedirectToAction("Index", "News").Success(message);
        }

        public ActionResult Edit(int id)
        {
            var news = _repo.GetNewsById(id);

            if (news == null)
                return HttpNotFound();

            var viewModel = new NewsFormViewModel()
            {
                Id = news.Id,
                Title = news.Title,
                Details = news.Details
            };
            if (news.NewsImages.Count > 0)
            {
                var img = news.NewsImages.FirstOrDefault();
                if (img != null) viewModel.Image = img.ImagePath;
            }
            return View("AddNews", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update(NewsFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("AddNews", model);


            var news = _repo.GetNewsById(model.Id);
            if (news == null)
                return HttpNotFound("News Not Exist!");
            news.Title = model.Title;
            news.Details = model.Details;

            if (news.NewsImages.Count > 0)
            {
                var img = news.NewsImages.FirstOrDefault();
                if (img != null) img.ImagePath = model.Image;
            }
            else if (!string.IsNullOrEmpty(model.Image))
            {
                 news.NewsImages.Add(new NewsImage()
                {
                    ImagePath = model.Image,
                    IsDefault = true,
                    NewsId = news.Id
                });
            }
            _repo.UpdateNews(news);

            var message = string.Format("تم تعديل الخبر  {0} بنجاح", news.Title);
            return RedirectToAction("Index", "News").Success(message);
        }

        

        public ActionResult DeleteNews(NewsSearchModel search)
        {



            try
            {
                var ads = _repo.GetNewsById(search.Id);
                if (ads == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                _repo.DeleteNews(search.Id);
                return RedirectToAction("Search", new { Page = search.Page, Keyword = search.SearchTerm , PublishingDate=search.PublishingDate });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return new HttpStatusCodeResult(404, "NotFound");


            }

        }

    }


}