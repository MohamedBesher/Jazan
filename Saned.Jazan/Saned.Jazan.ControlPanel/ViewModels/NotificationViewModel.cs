using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class NotificationViewModel      
    {
        [Required(ErrorMessage = "رقم الأعلان مطلوب .")]
        public int Id { get; set; }
        [Required(ErrorMessage = "محتوى الأشعار مطلوب .")]
        [StringLength(250, ErrorMessage = "محتوى الأشعار لا يزيد عن 250 حرف .")]

        public string Content { get; set; }
    }
}