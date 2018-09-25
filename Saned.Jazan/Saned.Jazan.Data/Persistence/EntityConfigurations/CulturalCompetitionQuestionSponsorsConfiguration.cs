using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class CulturalCompetitionQuestionSponsorsConfiguration : EntityTypeConfiguration<CulturalCompetitionQuestionSponsor>
    {
        public CulturalCompetitionQuestionSponsorsConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CreatedOn).IsRequired();
            Property(x => x.CreatedBy).IsRequired().HasMaxLength(128);
            
            HasRequired(x => x.Advertisement).WithMany(x => x.CulturalCompetitionQuestionSponsors).WillCascadeOnDelete(true);

            HasRequired(x => x.CulturalCompetitionQuestion).WithMany(x => x.CulturalCompetitionQuestionSponsors).WillCascadeOnDelete(true);

            HasRequired(x => x.CreatedByUser).WithMany(x => x.CulturalCompetitionQuestionSponsorCreatedBy).WillCascadeOnDelete(false);
            MapToStoredProcedures();
        }
    }
}
