using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [DataType(DataType.Password)]
        [Display(Name = "باسورد")]
        public string Password { get; set; }

        [Display(Name = "تذكرنى؟")]
        public bool RememberMe { get; set; }
    }
}