using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class Advertisement_SelectById_Result
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public int CategoryId { get; set; }
        public int PackageId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string WorkingHours { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string Twitter { get; set; }
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int ViewsCount { get; set; }

        public string AdvertisementImageUrl { get; set; }

        public int AdvertisementImageId { get; set; }
    }
}
