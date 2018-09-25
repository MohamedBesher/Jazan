using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Saned.Jazan.Api.Models
{
    public class TouristVisitViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        public DateTime VisitDate { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string MainImageBase64 { get; set; }
        public string MainImageExtension { get; set; }
        public List<ImageViewModel> Images { get; set; }
        public List<string> YouTubeUrls { get; set; }
    }
}