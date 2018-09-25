using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using PagedList;
using Saned.Common.Comments.Repository;
using Saned.Common.Notification.Repository;
using Saned.Jazan.ControlPanel.Extensions;
using Saned.Jazan.ControlPanel.Properties;
using Saned.Jazan.ControlPanel.ViewModels;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;
using Kendo.Mvc.Extensions;
using Saned.Jazan.ControlPanel.Error;

namespace Saned.Jazan.ControlPanel.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CompetitionController : Controller
    {
        // GET: Competition


        readonly ICulturalCompetitionQuestionRepository _repo;     
        readonly ICulturalCompetitionAnswersRepository _repoAnswer;
        readonly IAdvertisementRepository _repoAdvertisement;


        public CompetitionController( ICulturalCompetitionQuestionRepository question, ICulturalCompetitionAnswersRepository answer, IAdvertisementRepository repoAdvertisement)
        {
            this._repo = question;       
            this._repoAnswer = answer;       
            this._repoAdvertisement = repoAdvertisement;       
        }
        // GET: Advertisement
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Search(CulturalCompetitionQuestionSearchModel search)
        {
            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            int pageSize = Settings.Default.PageSize;
            ViewBag.Page = search.Page;
            var questions = _repo.GetAll().Where(u =>
                        (string.IsNullOrEmpty(search.Keyword) || u.Question.Contains(search.Keyword) || u.Title.Contains(search.Keyword))                        
                            ).OrderBy(u => u.CreatedOn); ;
            ViewBag.ResultCount = questions.Count();
            int result = (questions.Count() / pageSize) + (questions.Count() % pageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView(questions.ToPagedList(search.Page - 1, pageSize));
            }
            else
                return PartialView(questions.ToPagedList(search.Page, pageSize));
         
        }


        public ActionResult Details(int id)
        {
            CulturalCompetitionQuestion question = _repo.GetById(id);
            if (question == null)
                return RedirectToAction("Index", "Competition").Error("هذه المسابقة غير موجودة.");
            return View(question);
        }


        public ActionResult _GetUsersByQuestionId(CulturalCompetitionQuestionSearchModel search)
        {

            search.Page = search.Page == 0 ? 1 : search.Page;
            ViewBag.FrontEndUrl = Settings.Default.FrontEndUrl;
            ViewBag.Page = search.Page;
            var questions = _repo.GetUsersByQuestionId(search.Id).Where(u =>
                        (string.IsNullOrEmpty(search.Keyword) || u.Value.Contains(search.Keyword) )
                            ).OrderBy(u => u.CreatedOn); ;
            ViewBag.ResultCount = questions.Count();
            int result = (questions.Count() / search.PageSize) + (questions.Count() % search.PageSize > 0 ? 1 : 0);
            if (search.Page > 1 && result < search.Page)
            {
                ViewBag.Page = search.Page - 1;
                return PartialView("_GetUsersByQuestionId",questions.ToPagedList(search.Page - 1, search.PageSize));
            }
            else
                return PartialView("_GetUsersByQuestionId", questions.ToPagedList(search.Page, search.PageSize));
        }



        [System.Web.Mvc.HttpPost]

        public ActionResult Approve(int id)
        {
            try
            {
                var question = _repo.GetById(id);
                if (question == null)
                    return Json(new {message = "NotFound"});
                if (_repo.MarkAllAsUnPublished())
                {
                    question.IsPublished = true;
                    int result =_repo.UpdateQuestion(question);
                    if (result==1)
                       return Json(new {message = "Approved"});
                    else
                        return Json(new { message = "NotApproved" });

                }

                return Json(new { message = "NotApproved" });

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
                var question = _repo.GetById(id);
                if (question == null)
                    return Json(new { message = "NotFound" });
                question.IsPublished = false;
                int result =_repo.UpdateQuestion(question);
                if (result==1)
                   return Json(new { message = "UnApproved" });
                else
                    return Json(new { message = "NotUnApproved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);
                return Json(new { message = "NotUnApproved" });

            }

        }




        [System.Web.Mvc.HttpPost]

        public ActionResult SetAsWinner(int id)
        {
            try
            {
                CulturalCompetitionAnswer answer = _repo.GetAnswerById(id);
                if (answer == null)
                    return Json(new { message = "NotFound" });
                 answer.IsWinner = true;               
                    _repo.UpdateAnswer(answer);
                    return Json(new { message = "Approved" });
               

            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotApproved" });

            }

        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CancelWining(int id)
        {
            try
            {
                CulturalCompetitionAnswer answer = _repo.GetAnswerById(id);
                if (answer == null)
                    return Json(new { message = "NotFound" });
                answer.IsWinner = false;

                _repo.UpdateAnswer(answer);
                return Json(new { message = "UnApproved" });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return Json(new { message = "NotUnApproved" });

            }

        }

        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteAnswer(CulturalCompetitionQuestionSearchModel search)
        {
            try
            {
                var question = _repo.GetAnswerById(search.Id);
                if (question == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                _repo.DeleteAnswer(question);
                return RedirectToAction("_GetUsersByQuestionId", new { Id=search.QuestionId, Page = search.Page, Keyword = search.Keyword });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return new HttpStatusCodeResult(404, "NotFound");


            }

        }



        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(CulturalCompetitionQuestionSearchModel search)
        {
            try
            {
                var question = _repo.GetById(search.Id);
                if (question == null)
                    return new HttpStatusCodeResult(404, "NotFound");
                _repo.DeleteQuestion(question);
                return RedirectToAction("Search", new { Page = search.Page, Keyword = search.Keyword });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e);

                return new HttpStatusCodeResult(404, "NotFound");


            }

        }

        public ActionResult Add()
        {

            ViewBag.MultiselectCountry = GetCountries(null);
            return View();
        }

       [HttpGet]

        public ActionResult GetAdvertisementsStartsWith(string text)
        {
            
            return Json(GetAdvertisments(text),JsonRequestBehavior.AllowGet);
        }

        private IQueryable<Sponsor> GetAdvertisments(string text)
        {
            var advertisements = _repoAdvertisement.GetAll_Select(text)
                .OrderBy(u => u.CreatedOn); // return a list

            IQueryable<Sponsor> advertisemtsObj = advertisements.Select(m => new Sponsor
            {
                Value = m.Id,
                Text = m.Name,
                PhotoUrl = m.ImageUrl,
                CategoryName = m.Category.CategoryNameAr
            });
            return advertisemtsObj;
        }

        [HttpPost]
        public ActionResult Add(CompetitionQuestionViewModel model)
        {

            if (ModelState.IsValid)
            {

                CulturalCompetitionQuestion question = new CulturalCompetitionQuestion();
                question.Title = model.Title;
                question.Question = model.Question;
                question.IsPublished = true;
                DateTime questionCreatedOn = DateTime.Now;
                question.CreatedOn = questionCreatedOn;
                var questionCreatedBy = User.Identity.GetUserId();
                question.CreatedBy = questionCreatedBy;
              
                //int id = 10;


                if (model.Advertisements!=null && model.Advertisements.Count > 0)
                {

                    question.CulturalCompetitionQuestionSponsors = (from ads in model.Advertisements
                        select new CulturalCompetitionQuestionSponsor()
                        {
                            AdvertisementId = ads,
                            //CulturalCompetitionQuestionId = id,
                            CreatedOn = questionCreatedOn,
                            CreatedBy = questionCreatedBy
                        }).ToList();



                }

                bool result = _repo.Add(question);


                //;
                if (result)
                 return RedirectToAction("Index", "Competition").Success("تم اضافة المسابقة بنجاح");
                else
                    return RedirectToAction("Index", "Competition").Error("حدث خطأ اثناء الاضافة .");

            }
            return View(model);
        }


        private MultiSelectList GetCountries(string[] SelectedValues)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "India", Value = "1" });
            items.Add(new SelectListItem { Text = "China", Value = "2" });
            items.Add(new SelectListItem { Text = "United Sates", Value = "3" });
            items.Add(new SelectListItem { Text = "Srilanka", Value = "4" });
            items.Add(new SelectListItem { Text = "Germany", Value = "5" });
            items.Add(new SelectListItem { Text = "Japan", Value = "6" });
            items.Add(new SelectListItem { Text = "Nepal", Value = "7" });
            items.Add(new SelectListItem { Text = "Russia", Value = "8" });
            items.Add(new SelectListItem { Text = "Spain", Value = "9" });
            items.Add(new SelectListItem { Text = "Frans", Value = "10" });
            items.Add(new SelectListItem { Text = "Canada", Value = "11" });
            items.Add(new SelectListItem { Text = "brazil", Value = "12" });
            items.Add(new SelectListItem { Text = "Koria", Value = "13" });
            items.Add(new SelectListItem { Text = "England", Value = "14" });
            return new MultiSelectList(items, "Value", "Text", SelectedValues);
        }





        [HttpPost]
        public ActionResult Virtualization_Read([DataSourceRequest] DataSourceRequest request,string text)
        {
            
                 return Json(GetAdvertisments(text).ToDataSourceResult(request));
            
        }

        public ActionResult Orders_ValueMapper(int[] values)
        {
            var indices = new List<int>();

            if (values != null && values.Any())
            {
                var index = 0;

                foreach (var order in GetAdvertisments(""))
                {
                    if (values.Contains(order.Value))
                    {
                        indices.Add(index);
                    }

                    index += 1;
                }
            }

            return Json(indices, JsonRequestBehavior.AllowGet);
        }

       




        public ActionResult Edit(int id)
        {
            if (id > 0)
            {
                CulturalCompetitionQuestion question = _repo.GetById(id);
                CompetitionQuestionViewModel selected = AutoMapper.Mapper.Map<CulturalCompetitionQuestion,CompetitionQuestionViewModel>(question);

                selected.SelectedSponsors = _repo.GetSponsorsByAdsId(id).ToList();
               selected.Advertisements = selected.SelectedSponsors.Select(u => u.Value).ToList();



                return View(selected);


            }
            return RedirectToAction("Index", "Competition");


        }
        [HttpPost]
        public ActionResult Edit(CompetitionQuestionViewModel model)
        {

            if (ModelState.IsValid)
            {
                DateTime questionCreatedOn = DateTime.Now;
                var questionCreatedBy = User.Identity.GetUserId();
                CulturalCompetitionQuestion selected = AutoMapper.Mapper.Map<CompetitionQuestionViewModel, CulturalCompetitionQuestion>(model);
                selected.UpdatedBy = questionCreatedBy;
                selected.UpdatedOn = questionCreatedOn;


                if (model.Advertisements != null && model.Advertisements.Count > 0)
                {
                    selected.CulturalCompetitionQuestionSponsors = (from ads in model.Advertisements
                                                                    select new CulturalCompetitionQuestionSponsor()
                                                                    {
                                                                        AdvertisementId = ads,
                                                                        CreatedOn = questionCreatedOn,
                                                                        CreatedBy = questionCreatedBy
                                                                    }).ToList();
                }
                

                bool result = _repo.UpdateCulturalCompetitionQuestion(selected);
                if (result)
                    return RedirectToAction("Index", "Competition").Success("تم تعديل المسابقة بنجاح");
                else
                    return RedirectToAction("Index", "Competition").Error("حدث خطأ اثناء التعديل .");

            }
            return View(model);
        }

    }
}