using Saned.Jazan.Data.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Persistence.EntityConfigurations
{
    public class CulturalCompetitionQuestionConfiguration : EntityTypeConfiguration<CulturalCompetitionQuestion>
    {
        public CulturalCompetitionQuestionConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasMaxLength(250).IsRequired();
            Property(x => x.Question).HasMaxLength(800).IsRequired();
            Property(x => x.CreatedOn).IsRequired();
            Property(x => x.CreatedBy).IsRequired().HasMaxLength(128);
            Property(x => x.UpdatedBy).IsOptional().HasMaxLength(128);
            Property(x => x.IsPublished).IsRequired();

            HasRequired(x => x.CreatedByUser).WithMany(x => x.CulturalCompetitionQuestionCreatedBy).WillCascadeOnDelete(false);
            HasOptional(x => x.UpdatedByUser).WithMany(x => x.CulturalCompetitionQuestionUpdatedBy).WillCascadeOnDelete(false);

            MapToStoredProcedures();
             
        }
    }
}
