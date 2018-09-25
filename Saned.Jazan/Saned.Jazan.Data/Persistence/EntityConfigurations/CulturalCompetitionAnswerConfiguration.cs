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
    public class CulturalCompetitionAnswerConfiguration : EntityTypeConfiguration<CulturalCompetitionAnswer>
    {
        public CulturalCompetitionAnswerConfiguration()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.IsWinner).IsRequired();
            Property(x => x.Value).IsRequired().HasMaxLength(500);
            Property(x => x.CreatedOn).IsRequired();
            Property(x => x.CreatedBy).IsRequired();

            HasRequired(x => x.CulturalCompetitionQuestions).WithMany(q => q.CulturalCompetitionAnswers).WillCascadeOnDelete(true);
            HasRequired(x => x.CreatedByUser).WithMany(x => x.CulturalCompetitionAnswersCreatedBy).WillCascadeOnDelete(true);
            MapToStoredProcedures();
        }
    }
}
