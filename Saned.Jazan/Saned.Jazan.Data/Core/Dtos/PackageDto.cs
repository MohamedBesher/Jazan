using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class PackageDto
    {
        public int PackageId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public int Period { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public decimal Price { get; set; }
        public int OverallCount { get; set; }
    }

    public class PackageParamterDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public int? Period { get; set; }
        public decimal? Price { get; set; }
    }
}
