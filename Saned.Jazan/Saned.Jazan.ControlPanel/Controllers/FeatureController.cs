using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
    public class FeatureController : Controller
    {
            readonly IFeatureRepository _repo;
            public FeatureController(IFeatureRepository repo)
            {
                this._repo = repo;
            }
            // GET: Advertisement
            public ActionResult Index()
            {
                return View();
            }
            public PartialViewResult Search(FeatureSearchModel search)
            {
                search.Page = search.Page == 0 ? 1 : search.Page;
                ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize = Settings.Default.PageSize;
            ViewBag.keyword = search.Keyword;
                ViewBag.Page = search.Page;
                var features = _repo.All()
                    .Where(u =>
                              (string.IsNullOrEmpty(search.Keyword) || u.ArabicName.Contains(search.Keyword))
                                ).OrderBy(u=>u.Id);
                ViewBag.ResultCount = features.Count();
                return PartialView(features.ToPagedList(search.Page, pageSize));
            }


            //public ActionResult Details(int id)
            //{
            //    ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            //    Feature feature = _repo.SelectById(id);
            //    if (feature==null)
            //        return HttpNotFound();

            //    return View(feature);
            //}



            public ActionResult Edit(int id)
            {
                Feature feature = _repo.SelectById(id);
                return View(feature);

            }

            [HttpPost]
            public ActionResult Edit(Feature feature)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Feature selected = _repo.SelectById(feature.Id);
                        if (selected == null)
                            return new HttpStatusCodeResult(404, "NotFound");
                        _repo.UpdateFeature(feature);
                        return Json(new {Message = "OK"});
                    }
                return new HttpStatusCodeResult(404, "NotFound");

            }
            catch (Exception e)
                {
                ErrorSaver.SaveError(e);

                return new HttpStatusCodeResult(404, "NotFound");
                }


            }



        
    }
}