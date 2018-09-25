using Saned.Jazan.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class PackageFeatureConfiguration : EntityTypeConfiguration<PackageFeature>
    {
        public PackageFeatureConfiguration()
        {
            Property(x => x.PackageId).IsRequired();
            Property(x => x.FeatureId).IsRequired();
            HasRequired(x => x.Feature).WithMany(f => f.PackageFeatures).WillCascadeOnDelete(false);
            HasRequired(x => x.Package).WithMany(p => p.PackageFeatures).WillCascadeOnDelete(false);
            Property(x => x.Quantity).IsOptional();
            Property(x => x.Period).IsOptional();
        }
    }
}
