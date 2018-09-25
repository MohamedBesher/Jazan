using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class TouristVisit_SelectByIdResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string UserName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string YouTube { get; set; }
        public string MainImageBase64 { get; set; }
        public string MainImageExtension { get; set; }
        public string MediaUrl { get; set; }
        public MediaType? MediaType { get; set; }
        public int? TouristVisitImageId { get; set; }

        public int Rating { get; set; }

        public string CityName { get; set; }

        public DateTime VisitDate { get; set; }

        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }

    }
}
