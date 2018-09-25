using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class AdvertisementFeature
    {
        public int Id { get; set; }

        public int AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }
        
        public int FeatureId { get; set; }

        public Feature Feature { get; set; }
        
        public int? Period { get; set; }

        public int? Quantity { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
