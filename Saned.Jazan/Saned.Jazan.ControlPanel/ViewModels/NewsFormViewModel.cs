using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Saned.Jazan.ControlPanel.Controllers;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class NewsFormViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        [DisplayName("عنوان الخبر")]
        public string Title { get; set; }
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(3000, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        [DisplayName("تفاصيل الخبر")]
        public string Details { get; set; }
        [DisplayName("صورة الخبر")]
        public HttpPostedFileBase ImageUrl { get; set; }

        //[Required(ErrorMessage = "تأكد من إدخال صورة الخبر")]
        public string Image { get; set; }
        public string Action
        {
            get
            {
                Expression<Func<NewsController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<NewsController, ActionResult>> create = (c => c.AddNews(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }
    }
}