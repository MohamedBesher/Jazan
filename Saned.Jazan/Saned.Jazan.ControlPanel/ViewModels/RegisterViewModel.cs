using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(100, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفا", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }



       
    }
}