using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class PackageFeature
    {
        public int Id { get; set; }

        [Display(Name = "الباقة")]
        [Required(ErrorMessage = "يجب إدخال الباقة")]
        public int PackageId { get; set; }


        public Package Package { get; set; }

        [Display(Name = "الخاصية")]
        [Required(ErrorMessage = "يجب إدخال الخاصية")]
        public int FeatureId { get; set; }

        public Feature Feature { get; set; }

        [Display(Name = "المدة")]
        public int? Period { get; set; }

        [Display(Name = "العدد")]
        public int? Quantity { get; set; }
    }
}
