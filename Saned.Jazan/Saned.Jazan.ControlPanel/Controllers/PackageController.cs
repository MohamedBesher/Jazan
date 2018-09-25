using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microsoft.AspNet.Identity;
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
    public class PackageController : Controller
    {
        // GET: Package
       
           
            readonly IPackageRepository _repoPackage;
            public PackageController(IPackageRepository package)
            {
              
                this._repoPackage = package;
            }
            // GET: Advertisement
            public ActionResult Index(int page = 1)
            {
                return View();
            }
            public PartialViewResult Search(PackageSearchModel search)
            {
                search.Page = search.Page == 0 ? 1 : search.Page;
                ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
                int pageSize = Settings.Default.PageSize;
                ViewBag.keyword = search.Keyword;
                ViewBag.Page = search.Page;
               
                var packages = _repoPackage.GetAll()
                    .Where(u =>
                              (string.IsNullOrEmpty(search.Keyword) || u.ArabicName.Contains(search.Keyword))
                               
                                ).OrderBy(u=>u.Id);
                ViewBag.ResultCount = packages.Count();
                return PartialView(packages.ToPagedList(search.Page, pageSize));
            }


            public ActionResult Details(int id)
            {
                ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
                Package package =  _repoPackage.SelectByIdwithFeatures(id);
                return View(package);
            }

         
            public ActionResult Edit(int id)
            {
            Package package = _repoPackage.SelectByIdwithFeatures(id);
                if (package == null)
                    return RedirectToAction("Index", "Package").Error("هذه الباقة غير موجودة");
            return View(package);
           
            }

        [HttpPost]
        public ActionResult Edit(Package package)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                Package selected = _repoPackage.SelectPackageById(package.Id);
                if (selected == null)
                      return new HttpStatusCodeResult(404, "NotFound");            
                _repoPackage.UpdatePackage(package);
                return Json(new {Message="OK"});
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