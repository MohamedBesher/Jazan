using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class TouristVisitImagesConfigurations : EntityTypeConfiguration<TouristVisitImage>
    {
        public TouristVisitImagesConfigurations()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ImageUrl).IsRequired();
            Property(x => x.TouristVisitId).IsRequired();
            Property(x => x.MediaType).IsRequired();

            HasRequired(x => x.TouristVisit).WithMany(x => x.TouristVisitImages).WillCascadeOnDelete(true);

            MapToStoredProcedures();
        }
    }
}
