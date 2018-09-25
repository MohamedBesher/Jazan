using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Models
{
    public class CulturalCompetitionQuestionSponsor
    {
        public int Id { get; set; }
        public int CulturalCompetitionQuestionId { get; set; }
        public CulturalCompetitionQuestion CulturalCompetitionQuestion { get; set; }
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
