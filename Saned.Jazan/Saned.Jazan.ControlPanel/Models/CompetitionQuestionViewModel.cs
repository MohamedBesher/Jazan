using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Saned.Jazan.Data.Core.Models;
using Saned.Jazan.Data.Core.Repositories;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class CompetitionQuestionViewModel
    {
        [DisplayName("العنوان")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Title { get; set; }
        [DisplayName("السؤال")]
        [Required(ErrorMessage = "تأكد من إدخال" + " {0} ")]
        [StringLength(250, ErrorMessage = " {0} " + "لا يزيد عن " + " {1} " + "حرفاً")]
        public string Question { get; set; }

        [DisplayName("الراعين")]
        public List<int> Advertisements { get; set; }

        public int Id { get; set; }
        public IEnumerable<Sponsor> SelectedSponsors { get; set; }



        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsPublished { get; set; }
    }

}