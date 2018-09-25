using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class Advertisement
    {
        public int Id { get; set; }
        [Display(Name = "اسم الأعلان")]
        public string Name { get; set; }
        [Display(Name = "المدينة")]

        public string CityName { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int PackageId { get; set; }
        public virtual Package Package { get; set; }
        [Display(Name = "تفاصيل الأعلان")]

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "خط الطول")]

        public string Latitude { get; set; }
        [Display(Name = "دائرة العرض")]

        public string Longitude { get; set; }
        [Display(Name = "مواعيد العمل")]

        public string WorkingHours { get; set; }
        [Display(Name = "الجوال")]

        public string Mobile { get; set; }
        [Display(Name = "البريد الالكترونى")]

        public string Email { get; set; }
        [Display(Name = "الموقع الالكترونى")]

        public string WebSite { get; set; }
        [Display(Name = "تويتر")]

        public string Twitter { get; set; }
        [Display(Name = "فيس بوك")]

        public string FaceBook { get; set; }
        [Display(Name = "انستجرام")]

        public string Instagram { get; set; }
        [Display(Name = "سناب شات")]

        public string Snapchat { get; set; }
        [Display(Name = "حالة الموافقة")]

        public bool IsApproved { get; set; }
        [Display(Name = "حالة الاعلان")]

        public bool IsActive { get; set; }
        [Display(Name = "تاريخ الاضافة")]

        public DateTime CreatedOn { get; set; }
        [Display(Name = "المعلن")]

        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        [Display(Name = "تاريخ بداية الأعلان")]

        public DateTime? StartDate { get; set; }
        [Display(Name = "تاريخ نهاية الأعلان")]

        public DateTime? EndDate { get; set; }

        public virtual ICollection<AdvertisementImage> AdvertisementImages { get; set; }
        public virtual ICollection<AdvertisementFeature> AdvertisementFeatures { get; set; }
        public virtual ICollection<CulturalCompetitionQuestionSponsor> CulturalCompetitionQuestionSponsors { get; set; }

        [ForeignKey("CreatedBy")]

        public virtual ApplicationUser CreatedByUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser UpdatedByUser { get; set; }

        public Advertisement()
        {
            AdvertisementImages = new HashSet<AdvertisementImage>();
            AdvertisementFeatures = new HashSet<AdvertisementFeature>();
            CulturalCompetitionQuestionSponsors = new HashSet<CulturalCompetitionQuestionSponsor>();
        }

    }
}
