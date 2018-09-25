using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saned.Jazan.Data.Core.Models
{
    public class TouristVisit
    {
        public int Id { get; set; }
        [Display(Name = "اسم الزيارة")]
        public string Name { get; set; }
        [Display(Name = "اسم المدينة")]

        public string CityName { get; set; }
        [Display(Name = "تاريخ الزيارة")]

        public DateTime VisitDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        [Display(Name = "خط  الطول")]

        public string Latitude { get; set; }
        [Display(Name = "دائرة العرض")]

        public string Longitude { get; set; }
        [Display(Name = "الوصف")]

        public string Description { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser UpdatedByUser { get; set; }

        public virtual ICollection<TouristVisitImage> TouristVisitImages { get; set; }
        [Display(Name = "الحالة")]

        public bool IsApproved { get; set; } = false;
    }
}
