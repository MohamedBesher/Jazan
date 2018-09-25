using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    class AdvertisementImageConfiguration : EntityTypeConfiguration<AdvertisementImage>
    {
        public AdvertisementImageConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ImageUrl).IsRequired();
            HasRequired(x => x.Advertisement).WithMany(x => x.AdvertisementImages).WillCascadeOnDelete(true);
            MapToStoredProcedures();
        }
    }
}
