using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class TouristVisitConfiguration : EntityTypeConfiguration<TouristVisit>
    {
        public TouristVisitConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasMaxLength(250).IsRequired();
            Property(x => x.Description).HasMaxLength(1000);
            Property(x => x.CreatedOn).IsRequired();
            Property(x => x.CreatedBy).IsRequired();
            Property(x => x.CityName).IsRequired().HasMaxLength(250);
            Property(x => x.VisitDate).IsRequired();
            Property(x => x.VisitDate).IsRequired();

            HasRequired(x => x.CreatedByUser).WithMany(x => x.TouristVisitCreatedBy);
            HasOptional(x => x.UpdatedByUser).WithMany(x => x.TouristVisitUpdatedBy);

            MapToStoredProcedures();

        }
    }
}
