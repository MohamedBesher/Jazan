using System;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class AdvertisementDto
    {
        public int AdvertisementId { get; set; }
        public string AdvertisementName { get; set; }
        public string AdvertisementImageUrl { get; set; }
        public string CityName { get; set; }
        public string Description { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByUserName { get; set; }
        public string Email { get; set; }
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Mobile { get; set; }
        public string Snapchat { get; set; }
        public string Twitter { get; set; }
        public string WebSite { get; set; }
        public string WorkingHours { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string Features { get; set; }
        public int OverallCount { get; set; }
        public int CategoryId { get; set; }
        public bool IsApproved { get; set; }
        public string Images { get; set; }
        public int? Rating { get; set; }
        public int? Views { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CategoryName { get; set; }
    }
}
