using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using PagedList;
using Saned.Jazan.Admin.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;

namespace Saned.Jazan.Admin.Controllers
{
    public class AdvertisementController : Controller
    {

        private IUnityContainer unitycontainer;
        private IAdvertisementRepository _addRepository;
        private ICategoryRepository _categoryRepository;
        private IPackageRepository _packageRepository;
          private AuthRepository _authRepository;

        public AdvertisementController()
        {
            unitycontainer = new UnityContainer();
            unitycontainer.RegisterType<IAdvertisementRepository, AdvertisementRepository>();
            _addRepository = unitycontainer.Resolve<IAdvertisementRepository>();
            unitycontainer.RegisterType<ICategoryRepository, CategoryRepository>();
            _categoryRepository = unitycontainer.Resolve<ICategoryRepository>();
            unitycontainer.RegisterType<IPackageRepository, PackageRepository>();
            _packageRepository = unitycontainer.Resolve<IPackageRepository>();
            
            _authRepository =new AuthRepository();
        }
        //public ActionResult Index(int? page)
        //{
        // var ads=   _addRepository.GetAll().Select(x=> (AdvertisementModel)x);
        //    return View(ads.ToPagedList(page ?? 1, 10));
        //}
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult AjaxHandler(DataTableParamModel param)
        {
            var ads = _addRepository.GetAll().Select(x => (AdvertisementModel)x);
            var advertisementModels = ads as IList<AdvertisementModel> ?? ads.ToList();         
            IEnumerable<AdvertisementModel> filteredData = GetSearchResult(param.sSearch, advertisementModels.ToList());
            var result = from c in filteredData
                         select new[] {  c.Name, c.CityName
                       ,c.ImageUrl,c.WorkingHours,c.Mobile,c.Email,c.WebSite,c.Twitter,c.FaceBook,
                             c.Instagram,c.Snapchat,Convert.ToString(c.Id),Convert.ToString(c.Id),Convert.ToString(c.Id)};
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = advertisementModels.Count(),
                iTotalDisplayRecords = filteredData.Count(),
                aaData = result.Skip(param.iDisplayStart)
                        .Take(param.iDisplayLength)

        }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(int? id)
        {
            FillViewBag();
            if (id != null)
            {
                var adsItem = (AdvertisementModel)_addRepository.SelectAdId(id.Value);
                return View(adsItem);
            }
            return View();
        }


        [HttpPost]
        public ActionResult Create(AdvertisementModel model)
        {
            FillViewBag();
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    model.UpdatedOn = DateTime.Now.ToString();
                    _addRepository.Update(model);
                }
                else
                {
                    _addRepository.Create(model);
                }
                return RedirectToAction("Index", "Advertisement");
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult Details(int id)
        {
            FillViewBag();
            var adsItem = (AdvertisementModel)_addRepository.SelectAdId(id);
            return View(adsItem);
        }

        public ActionResult Delete(int id)
        {
            return Json(_addRepository.DeleteAd(id),JsonRequestBehavior.AllowGet);
        }

        private List<AdvertisementModel> GetSearchResult(string sSearch,  List<AdvertisementModel> dtResult)
        {
            if (!string.IsNullOrEmpty(sSearch))
            {
                return
                    dtResult.Where(
                        c => c.Name.IfNotNull(x=>x.Contains(sSearch)) || c.CityName.IfNotNull(x => x.Contains(sSearch)) || c.WorkingHours.IfNotNull(x => x.Contains(sSearch)) || c.Mobile.IfNotNull(x => x.Contains(sSearch)) ||
                             c.Email.IfNotNull(x => x.Contains(sSearch)) || c.WebSite.IfNotNull(x => x.Contains(sSearch)) 
                             || c.Twitter.IfNotNull(x => x.Contains(sSearch)) || c.FaceBook.IfNotNull(x => x.Contains(sSearch)) || c.Instagram.IfNotNull(x => x.Contains(sSearch)) || c.Snapchat.IfNotNull(x => x.Contains(sSearch))).ToList();
            }
            else
            {
                return dtResult;
            }
        }

        private void FillViewBag()
        {
            ViewBag.Categories = _categoryRepository.GetAll();
            ViewBag.Pakages = _packageRepository.GetAll();
            ViewBag.Users = _authRepository.GetAll();
        }
    }
}