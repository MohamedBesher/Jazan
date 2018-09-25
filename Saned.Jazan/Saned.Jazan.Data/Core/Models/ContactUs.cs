using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class MobileSetting
    {
        public int Id { get; set; }

        public SettingTypeEnum SettingType { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(250, ErrorMessage = "لا يزيد عن 250 حرفاً")]
        public string Value { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        [StringLength(250, ErrorMessage = "لا يزيد عن 250 حرفاً")]
        public string Name { get; set; }
    }
}
