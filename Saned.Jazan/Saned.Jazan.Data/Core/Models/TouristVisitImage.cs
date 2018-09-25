using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class TouristVisitImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public MediaType MediaType { get; set; }
        public int TouristVisitId { get; set; }
        public virtual TouristVisit TouristVisit { get; set; }
    }
}
