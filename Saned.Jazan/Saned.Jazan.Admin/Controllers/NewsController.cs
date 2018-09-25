using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Saned.Jazan.Admin.Models;
using Saned.Jazan.Data.Core.Repositories;
using Saned.Jazan.Data.Persistence.Repositories;
using WebGrease.Css.Extensions;
using PagedList;

namespace Saned.Jazan.Admin.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController()
        {
            IUnityContainer unitycontainer = new UnityContainer();
            unitycontainer.RegisterType<INewsRepository, NewsRepository>();
            _newsRepository = unitycontainer.Resolve<INewsRepository>();
        }

        // GET: News
        public  ActionResult Index(int? page)
        {
            var news = _newsRepository.GetAll();
            return View(news.ToPagedList(page ?? 1, 10));
        }

        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                var newsItem = (NewsModels)_newsRepository.GetNewsById(id.Value) ;
                return View(newsItem);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(NewsModels model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                    _newsRepository.UpdateNews(model);
                else
                    _newsRepository.SaveNews(model);
                return RedirectToAction("Index", "News");
            }
            else
            {
                return View(model);
            }
        }       
        public ActionResult _Images(int? newsId, int index = 0, NewsImagesModel newsImagesModel = null, ModelStateDictionary modelState = null)
        {
            ViewBag.ImageIndex = index;
            
          
            if (modelState != null)
                {
                    var related =
                        modelState.Keys.Where(
                            key =>
                                key.ToLower().StartsWith(string.Format("NewsImagesModel[{0}]", index)) &&
                                modelState[key].Errors != null);
                    related.ForEach(
                        key =>
                            modelState[key].Errors.ForEach(
                                e =>
                                    ModelState.AddModelError(
                                        key.Replace(string.Format("NewsImagesModel[{0}].", index), ""),
                                        e.ErrorMessage)));
                }



                return PartialView(newsImagesModel ?? new NewsImagesModel());
           
        }
        public ActionResult Delete(int id)
        {
            _newsRepository.DeleteNews(id);
            return RedirectToAction("Index");
        }
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/profile"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return View("Index");
        }
    }
}