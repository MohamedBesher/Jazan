using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saned.Jazan.Api.Models
{
    public class TouristVisitGetPagedParam
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string UserId { get; set; }

        public bool? IsApproved { get; set; }
    }
}