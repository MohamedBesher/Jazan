using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Saned.Jazan.ControlPanel.Controllers;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }
        public string ParentName { get; set; }
   

      
        public HttpPostedFileBase ImageUrl { get; set; }
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [DisplayName("صورة التصنيف")]
        public string Image { get; set; }
        public string Action
        {
            get
            {
                Expression<Func<CategoriesController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<CategoriesController, ActionResult>> create = (c => c.AddCategory(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(50, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        [DisplayName("أسم التصنيف")]
        public string CategoryNameAr { get; set; }

        public DateTime CreateDate { get; set; }

        public string ActionSub
        {
            get
            {
                Expression<Func<CategoriesController, ActionResult>> update = (c => c.UpdateSubCategory(this));
                Expression<Func<CategoriesController, ActionResult>> create = (c => c.AddSubCategory(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [DisplayName("التصنيف الرئيسى ")]

        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}