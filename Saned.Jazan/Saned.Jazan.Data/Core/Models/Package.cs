using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class Package
    {
        public int Id { get; set; }

        [Display(Name = "اسم الباقة العربي")]
        [Required(ErrorMessage = "يجب إدخال اسم الباقة العربي")]
        [StringLength(250)]
        public string ArabicName { get; set; }

        [Display(Name = "اسم الباقة الانجليزي")]
        [StringLength(250)]
        public string EnglishName { get; set; }

        [Display(Name = "المدة")]
        [Required(ErrorMessage = "يجب إدخال المدة ")]
        public int Period { get; set; }

        [Display(Name = "تاريخ الانشاء")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "تم الانشاء بواسطة")]
        [Required]
        public Guid CreatedBy { get; set; }

        [Display(Name = "تاريخ التعديل")]
        public DateTime? UpdatedOn { get; set; }

        [Display(Name = "تم التعديل بواسطة")]
        public Guid? UpdatedBy { get; set; }

        [Display(Name = "السعر")]
        public decimal Price { get; set; }

        public virtual ICollection<PackageFeature> PackageFeatures { get; set; }

        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
