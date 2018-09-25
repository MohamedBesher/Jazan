using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class MobileSettingViewModel
    {
        [DisplayName("عن التطبيق")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]

        public string AboutUs { get; set; }
        [DisplayName("رقم الهاتف")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "رقم هاتف غير صحيح")]


        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(12, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]

        public string MobileNumber { get; set; }
        [DisplayName("لاين")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(20, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Line { get; set; }
        [DisplayName("تويتر")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(100, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Twitter { get; set; }
        [DisplayName("انستجرام")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Instgram { get; set; }
        [DisplayName("سناب شات")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Snapchat { get; set; }
        [DisplayName("بلاك بيرى")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string BlackBerry { get; set; }
        [DisplayName("البريد الالكترونى")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        [EmailAddress(ErrorMessage = "ادخل بريد الكترونى صالح ")]

        public string Email { get; set; }
        [DisplayName("شروط المسابقة")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(500, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]

        public string CompitionConditions { get; set; }
    }
}