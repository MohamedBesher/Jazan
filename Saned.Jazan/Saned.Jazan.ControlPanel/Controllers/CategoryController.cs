using System;
using System.Linq;
using System.Web.Mvc;
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

    public class CategoriesController : Controller
    {

        readonly ICategoryRepository _repo;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this._repo = categoryRepository;
        }

        public ActionResult Index()
        {
            ViewData["Categories"] = new SelectList(_repo.GetAll().ToList(), "CategoryId", "CategoryNameAr");
            return View();
        }


        public JsonResult GetMainCategories()
        {
           
            return Json(_repo.GetAll().ToList(), JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Search(CategorySearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize = Settings.Default.PageSize;
            ViewBag.Page = search.Page;
            ViewBag.CategoryId = search.CategoryId;

            //int? paretId = null;
            //if (search.CategoryId != 0)
            //    paretId = search.CategoryId;

            var categories = _repo.GetSubCategories(search.CategoryId);

            ViewBag.ResultCount = categories.Count();

            int result = (categories.Count() / pageSize) + (categories.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(categories.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(categories.ToPagedList(search.Page, pageSize));



            //return PartialView(data);
        }


        public ActionResult Category()
        {
            return View();
        }

        public PartialViewResult SearchCategory(CategorySearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize = Settings.Default.PageSize;
            ViewBag.Page = search.Page;
            ViewBag.Keyword = search.Keyword;
            //ViewBag.CategoryId = search.CategoryId;

            int? paretId = null;
            if (search.CategoryId != 0)
                paretId = search.CategoryId;

            var categories = _repo.GetMainCategories()
                .Where(u=>
                        (string.IsNullOrEmpty(search.Keyword) || u.CategoryNameAr.Contains(search.Keyword)));

            ViewBag.ResultCount = categories.Count();

            int result = (categories.Count() / pageSize) + (categories.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(categories.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(categories.ToPagedList(search.Page, pageSize));



            //return PartialView(data);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(CategorySearchModel search)
        {
            try
            {
                var category = _repo.GetCategoryById(search.CategoryId);

                if (category == null)
                    return HttpNotFound();
                if (_repo.IsRelatedWith(search.CategoryId))
                    return new HttpStatusCodeResult(404, "Not Deleted");

                _repo.DeleteCategory(search.CategoryId);
                // return Json(new { message = "Deleted" });
                return RedirectToAction("Search", new { Page = search.Page}).Success("تم حذف التصنيف بنجاح .");
            }
            catch (Exception ex)
            {
                ErrorSaver.SaveError(ex);
                return new HttpStatusCodeResult(404,ex.Message);
            }
           

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteMainCategory(CategorySearchModel search)
        {
            try
            {
                var category = _repo.GetCategoryById(search.CategoryId);

                if (category == null)
                    return HttpNotFound();
                if (_repo.IsRelatedWith(search.CategoryId))
                    return new HttpStatusCodeResult(404, "Not Deleted");

                _repo.DeleteCategory(search.CategoryId);
                // return Json(new { message = "Deleted" });
                return RedirectToAction("SearchCategory", new { Page = search.Page, Keyword = search.Keyword }).Success("تم حذف التصنيف بنجاح .");
            }
            catch (Exception ex)
            {
                ErrorSaver.SaveError(ex);
                return new HttpStatusCodeResult(404, ex.Message);
            }


        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddCategory(CategoryFormViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return View("AddCategory", model);
            }

            var category = new Category
            {

                CategoryNameAr = model.CategoryNameAr,
                CategoryNameEn = model.CategoryNameAr,
                CreateDate = DateTime.Now,
                CategoryImage = model.Image,
                ParentId = 0

            };

            _repo.SaveCategory(category);



            var message = string.Format("تم إضافة التصنيف  {0} بنجاح", category.CategoryNameAr);

            return RedirectToAction("Category", "Categories").Success(message);
        }
        public ActionResult AddSubCategory()
        {
            var viewModel = new CategoryFormViewModel()
            {
                Categories = _repo.GetAll()
            };

            return View("AddSubCategory", viewModel);
        }
        public ActionResult AddCategory()
        {
            var viewModel = new CategoryFormViewModel()
            {
                Categories = _repo.GetAll()
            };

            return View("AddCategory", viewModel);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddSubCategory(CategoryFormViewModel model)
        {
            try
            {
                     model.Categories = _repo.GetAll();

                    if (!ModelState.IsValid)
                    {

                        return View("AddSubCategory", model);
                    }

                    var category = new Category
                    {
                        CategoryNameAr = model.CategoryNameAr,
                        CategoryNameEn = model.CategoryNameAr,
                        CreateDate = DateTime.Now,
                        CategoryImage = model.Image,
                        ParentId = model.CategoryId

                    };

                    _repo.SaveCategory(category);
                    var message = string.Format("تم إضافة التصنيف  {0} بنجاح", category.CategoryNameAr);
                    return RedirectToAction("Index", "Categories").Success(message);
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return View("AddSubCategory", model);
            }
           
        }
        public ActionResult Edit(int id)
        {
            var category = _repo.GetCategoryById(id);

            if (category == null)
                return HttpNotFound();

            var viewModel = new CategoryFormViewModel()
            {
                Categories = _repo.GetAll(),
                Id = category.CategoryId,
                CategoryNameAr = category.CategoryNameAr,
                CreateDate = DateTime.Now,
                Image = category.CategoryImage,

            };

            return View("AddCategory", viewModel);
        }
        public ActionResult EditSubCategory(int id)
        {
            var category = _repo.GetCategoryById(id);

            if (category == null)
                return HttpNotFound();


            var viewModel = new CategoryFormViewModel()
            {
                Categories = _repo.GetAll(),
                Id = category.CategoryId,
                CategoryNameAr = category.CategoryNameAr,
                CreateDate = DateTime.Now,
                Image = category.CategoryImage,
                CategoryId = category.ParentId
            };
            //var parentcategory = _repo.GetCategoryById(category.ParentId);

            //viewModel.ParentName = parentcategory.CategoryNameAr;
            return View("AddSubCategory", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("AddCategory", model);


            var category = _repo.GetCategoryById(model.Id);
            if (category == null)
                return HttpNotFound("Category Not Exist!");
            category.CategoryNameAr = model.CategoryNameAr;
            category.CategoryImage = model.Image;
            _repo.UpdateCategory(category);

            var message = string.Format("تم تعديل التصنيف  {0} بنجاح", category.CategoryNameAr);
            return RedirectToAction("Category", "Categories").Success(message);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateSubCategory(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("AddSubCategory", model);


            var category = _repo.GetCategoryById(model.Id);
            if (category == null)
                return HttpNotFound("Category Not Exist!");
            category.CategoryNameAr = model.CategoryNameAr;
            category.CategoryImage = model.Image;
            category.ParentId = model.CategoryId;
            _repo.UpdateCategory(category);

            var message = string.Format("تم تعديل التصنيف  {0} بنجاح", category.CategoryNameAr);
            return RedirectToAction("Index", "Categories").Success(message);
        }
    }
}