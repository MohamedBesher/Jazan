using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Saned.Jazan.Api.Models
{
    public class AdvertisementViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string CityName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PackageId { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
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
        public string MainImageBase64 { get; set; }
        public string MainImageExtension { get; set; }

        public List<ImageViewModel> AdvertisementImages { get; set; }
    }
}