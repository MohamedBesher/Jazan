using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class AdvertisementParam
    {
        [Required]
        public int PageSize { get; set; }
        [Required]
        public int PageNumber { get; set; }
        public int? CategoryId { get; set; }
        public string UserId { get; set; }
        public int? FeatureId { get; set; }
        public int? AdvertisementId { get; set; }
        public bool? IsApproved { get; set; }
    }
}