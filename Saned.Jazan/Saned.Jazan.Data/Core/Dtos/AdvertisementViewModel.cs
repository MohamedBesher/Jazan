using System;
using System.Collections.Generic;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class AdvertisementsViewModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public int PackageId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Package Package { get; set; }
        public Category Category { get; set; }
        public List<AdvertisementFeature> AdvertisementFeatures { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
    }
}