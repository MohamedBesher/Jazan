using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saned.Jazan.Data.Core.Models
{
    public class CulturalCompetitionAnswer
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int CulturalCompetitionQuestionId { get; set; }
        public virtual CulturalCompetitionQuestion CulturalCompetitionQuestions { get; set; }
        public bool IsWinner { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
