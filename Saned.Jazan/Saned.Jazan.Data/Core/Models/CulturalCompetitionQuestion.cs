using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saned.Jazan.Data.Core.Models
{
    public class CulturalCompetitionQuestion
    {
        
        public int Id { get; set; }
        [DisplayName("العنوان")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Title { get; set; }
        [DisplayName("السؤال")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]


        public string Question { get; set; }
        [DisplayName("تاريخ الاضافة")]
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsPublished { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser CreatedByUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser UpdatedByUser { get; set; }

        public virtual ICollection<CulturalCompetitionAnswer> CulturalCompetitionAnswers { get; set; }

        public virtual ICollection<CulturalCompetitionQuestionSponsor> CulturalCompetitionQuestionSponsors { get; set; }

        public CulturalCompetitionQuestion()
        {
            CulturalCompetitionAnswers = new HashSet<CulturalCompetitionAnswer>();
            CulturalCompetitionQuestionSponsors = new HashSet<CulturalCompetitionQuestionSponsor>();
        }
    }
}
