using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر الحالية")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(100, ErrorMessage = " {0} " + "لا تقل عن " + " {2} " + "حرفا", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر الجديدة ")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة السر")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور الجديدة لا تتطابق.")]
        public string ConfirmPassword { get; set; }
    }
}