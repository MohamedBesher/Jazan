using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryNameAr { get; set; }
        public string CategoryNameEn { get; set; }
        public string CategoryImage { get; set; }
        public int ParentId { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
   
        public virtual ICollection<Advertisement> Advertisements { get; set; }

        public Category()
        {
            Advertisements = new HashSet<Advertisement>();
        }
    }
}
