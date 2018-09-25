using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AboutUsController : Controller
    {
        readonly IMobileSettingRepository _repo;
        public AboutUsController(IMobileSettingRepository repo)
        {
            this._repo = repo;
        }


        public ActionResult AboutUs()
        {         
            var settings = Task.Run(() => _repo.GetAll()).Result.OrderBy(u=>u.Id);

            MobileSettingViewModel view = new MobileSettingViewModel();
            view.AboutUs = settings.FirstOrDefault(u => u.Id == 1)?.Value;
            view.MobileNumber = settings.FirstOrDefault(u => u.Id == 2)?.Value;
            view.Line= settings.FirstOrDefault(u => u.Id == 3)?.Value;
            view.Twitter = settings.FirstOrDefault(u => u.Id == 4)?.Value;
            view.Instgram = settings.FirstOrDefault(u => u.Id == 5)?.Value;
            view.Snapchat = settings.FirstOrDefault(u => u.Id == 6)?.Value;
            view.BlackBerry = settings.FirstOrDefault(u => u.Id == 7)?.Value;
            view.Email = settings.FirstOrDefault(u => u.Id == 8)?.Value;
            view.CompitionConditions = settings.FirstOrDefault(u => u.Id == 9)?.Value;

            return View(view);
        }


        [HttpPost]
        public ActionResult AboutUs(MobileSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.GetById(1).Value= model.AboutUs;
                _repo.GetById(2).Value = model.MobileNumber;
                 _repo.GetById(3).Value= model.Line;
                 _repo.GetById(4).Value= model.Twitter;
                 _repo.GetById(5).Value= model.Instgram ;
                 _repo.GetById(6).Value= model.Snapchat;
                 _repo.GetById(7).Value = model.BlackBerry;
                 _repo.GetById(8).Value= model.Email ;
                 _repo.GetById(9).Value= model.CompitionConditions ;
                _repo.SaveMobileSetting();
                return RedirectToAction("Index","Home").Success("تم الحفظ معلومات الاتصال بنجاح");

            }
            return View("AboutUs", model).Error("حدث خطأ اثناء الحفظ");
        }
        




        //E:\saned\Jazan\Saned.Jazan\Saned.Jazan.ControlPanel\Controllers\NewsController.cs


        //    var mobileSettingEnum =
        //{
        //    aboutUs: { id: 1, icon: '' },
        //    mobileNumber: { id: 2, icon: 'whatsapp.png' },
        //    line: { id: 3, icon: 'line.png' },
        //    twitter: { id: 4, icon: 'tw.png' },
        //    instgram: { id: 5, icon: 'instagram.png' },
        //    snapchat: { id: 6, icon: 'snapchat.png' },
        //    blackBerry: { id: 7, icon: 'bbm.png' },
        //    email: { id: 8, icon: 'telegram.png' }
        //};

    }


}