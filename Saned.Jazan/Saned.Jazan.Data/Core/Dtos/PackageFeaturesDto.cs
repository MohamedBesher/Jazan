using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class PackageFeaturesDto
    {
        public int PackageId { get; set; }
        public string PackageArabicName { get; set; }
        public string PackageEnglishName { get; set; }
        public int? Period { get; set; }
        public decimal Price { get; set; }
        public int FeatureId { get; set; }
        public string FeatureArabicName { get; set; }
        public string FeatureEnglishName { get; set; }
        public int? PackageFeaturePeriod { get; set; }
        public int? PackageFeatureQuantity { get; set; }
    }
}
