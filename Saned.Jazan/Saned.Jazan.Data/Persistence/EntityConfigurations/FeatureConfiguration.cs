using Saned.Jazan.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    /// <summary>
    /// Configure Feature Entity
    /// </summary>
    public class FeatureConfiguration : EntityTypeConfiguration<Feature>
    {
        public FeatureConfiguration()
        {
            
            Property(x => x.ArabicName).IsRequired();
            Property(x => x.ArabicName).HasMaxLength(250);
        }
    }
}
