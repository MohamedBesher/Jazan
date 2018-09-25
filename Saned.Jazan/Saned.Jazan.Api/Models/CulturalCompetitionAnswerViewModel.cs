using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Saned.Jazan.Api.Models
{
    public class CulturalCompetitionAnswerViewModel 
    {
        public int QuestionId { get; set; }
        [Required]
        [StringLength(500)]
        public string Value { get; set; }
    }
}