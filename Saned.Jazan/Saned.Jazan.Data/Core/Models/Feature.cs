using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }

        [Display(Name = "اسم الخاصية العربي")]
        [Required(ErrorMessage = "اسم الخاصية مطلوب")]
        [StringLength(maximumLength: 250, ErrorMessage = "اسم الخاصيةالعربي لا يجب ان يزيد عن 250 حرف")]
        public string ArabicName { get; set; }

        [Display(Name = "اسم الخاصية الانجليزي")]
        [StringLength(maximumLength: 250, ErrorMessage = "اسم الخاصيةالانجليزي لا يجب ان يزيد عن 250 حرف")]

        public string EnglishName { get; set; }

        public virtual ICollection<PackageFeature> PackageFeatures { get; set; }

        public virtual ICollection<AdvertisementFeature> AdvertisementFeatures { get; set; }
    }
}
