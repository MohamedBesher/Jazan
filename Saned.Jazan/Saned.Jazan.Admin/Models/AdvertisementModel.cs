using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.Admin.Models
{
    public class AdvertisementModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string CityName { get; set; }
        [Required]
        [Display(Name = "CategoryId")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "PackageId")]
        public int PackageId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        [Required]
        public string WorkingHours { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Twitter { get; set; }
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public string CreatedOn { get; set; } = DateTime.Now.ToString();
        [Required]
        public string CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        [Required]
        public string UpdatedBy { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public string StartDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public string EndDate { get; set; }
        public string CategoryName { get; set; }
        public string PackageName { get; set; }

        public virtual ICollection<AdvertisementImageModel> AdvertisementImagesModel { get; set; }
        public virtual ICollection<AdvertisementFeatureModel> AdvertisementFeaturesModel { get; set; }
        public virtual ICollection<CulturalCompetitionQuestionSponsorModel> CulturalQuestionSponsorModel { get; set; }
        


        public static implicit operator Advertisement(AdvertisementModel model)
        {

            return new Advertisement()
            {
                Id = model.Id,
                Name = model.Name,
                CityName = model.CityName,
                CategoryId = model.CategoryId,
                PackageId = model.PackageId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                WorkingHours = model.WorkingHours,
                Mobile = model.Mobile,
                Email = model.Email,
                WebSite = model.WebSite,
                Twitter = model.Twitter,
                FaceBook = model.FaceBook,
                Instagram = model.Instagram,
                Snapchat = model.Snapchat,
                IsApproved = model.IsApproved,
                IsActive = model.IsActive,
                CreatedOn = DateTime.Parse(model.CreatedOn),
                CreatedBy = model.CreatedBy,
                UpdatedOn = model.Id>0?DateTime.Parse(model.UpdatedOn): (DateTime?) null,
                UpdatedBy = model.UpdatedBy,
                StartDate = DateTime.Parse(model.StartDate),
                EndDate = DateTime.Parse(model.EndDate),
            };
        }

        public static explicit operator AdvertisementModel(Advertisement model)
        {
            return new AdvertisementModel()
            {
                Id = model.Id,
                Name = model.Name,
                CityName = model.CityName,
                CategoryId = model.CategoryId,
                PackageId = model.PackageId,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                WorkingHours = model.WorkingHours,
                Mobile = model.Mobile,
                Email = model.Email,
                WebSite = model.WebSite,
                Twitter = model.Twitter,
                FaceBook = model.FaceBook,
                Instagram = model.Instagram,
                Snapchat = model.Snapchat,
                IsApproved = model.IsApproved,
                IsActive = model.IsActive,
                CreatedOn = model.CreatedOn.ToString(),
                CreatedBy = model.CreatedBy,
                UpdatedOn = model.UpdatedOn.ToString(),
                UpdatedBy = model.UpdatedBy,
                StartDate =model.StartDate.ToString(),
                EndDate = model.EndDate.ToString(),
                CategoryName = model.Category.CategoryNameAr,
                PackageName = model.Package.ArabicName
            };
        }
    }
}