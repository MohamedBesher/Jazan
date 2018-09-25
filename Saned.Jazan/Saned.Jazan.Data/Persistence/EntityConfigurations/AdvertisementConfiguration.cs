using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class AdvertisementConfiguration : EntityTypeConfiguration<Advertisement>
    {
        public AdvertisementConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).HasMaxLength(250);
            Property(x => x.CityName).HasMaxLength(250);
            Property(x => x.ImageUrl).HasMaxLength(250).IsRequired();
            Property(x => x.Latitude).HasMaxLength(250);
            Property(x => x.Longitude).HasMaxLength(250);
            Property(x => x.Mobile).HasMaxLength(250);
            Property(x => x.Email).HasMaxLength(250);
            Property(x => x.WebSite).HasMaxLength(250);
            Property(x => x.Twitter).HasMaxLength(250);
            Property(x => x.FaceBook).HasMaxLength(250);
            Property(x => x.Snapchat).HasMaxLength(250);
            Property(x => x.Instagram).HasMaxLength(250);
            Property(x => x.Description).HasMaxLength(1000);
           
            Property(x => x.IsApproved).IsRequired();
            Property(x => x.WorkingHours).IsRequired();
            Property(x => x.IsActive).IsRequired();

            Property(x => x.CreatedBy).IsRequired().HasMaxLength(128);

            Property(x => x.UpdatedBy).HasMaxLength(128).IsOptional();
            HasRequired(x => x.CreatedByUser).WithMany(x => x.AdvertisementCreatedByUser).WillCascadeOnDelete(false);

            HasOptional(x => x.UpdatedByUser).WithMany(x => x.AdvertisementUpdatedByUser).WillCascadeOnDelete(false);

            HasRequired(x => x.Package).WithMany(p => p.Advertisements).WillCascadeOnDelete(true);
            HasRequired(x => x.Category).WithMany(c => c.Advertisements).WillCascadeOnDelete(true);
            MapToStoredProcedures();
            
        }
    }
}
