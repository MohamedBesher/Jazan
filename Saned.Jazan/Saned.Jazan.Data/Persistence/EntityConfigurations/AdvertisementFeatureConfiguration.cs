using Saned.Jazan.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class AdvertisementFeatureConfiguration : EntityTypeConfiguration<AdvertisementFeature>
    {
        public AdvertisementFeatureConfiguration()
        {
            Property(x => x.AdvertisementId).IsRequired();
            Property(x => x.FeatureId).IsRequired();
            HasRequired(x => x.Feature).WithMany(f => f.AdvertisementFeatures).WillCascadeOnDelete(false);
            HasRequired(x => x.Advertisement).WithMany(p => p.AdvertisementFeatures).WillCascadeOnDelete(true);
            Property(x => x.Quantity).IsOptional();
            Property(x => x.Period).IsOptional();
            MapToStoredProcedures();
        }
    }
}
