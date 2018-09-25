using System;
using System.Collections;
using System.Collections.Generic;

namespace Saned.Jazan.Data.Core.Models
{
    public partial class ApplicationUser
    {
        public ApplicationUser()
        {
            IsDeleted = false;
            IsApprove = false;
            CreatedDate = DateTime.Now;
        }

        public string Name { get; set; }
        public string SoicalMediaId { get; set; }
        public string PhotoUrl { get; set; }
        public string ConfirmedEmailToken { get; set; }
        public string ResetPasswordlToken { get; set; }
        public bool? IsSelfAdded { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; private set; }

        public void Archive()
        {
            IsDeleted = true;
        }
        public void UnArchive()
        {
            IsDeleted = false;
        }
        public void Approve()
        {
            IsApprove = true;
        }
        public void UnApprove()
        {
            IsApprove = false;
        }

        public virtual ICollection<CulturalCompetitionAnswer> CulturalCompetitionAnswersCreatedBy { get; set; }
        public virtual ICollection<CulturalCompetitionQuestionSponsor> CulturalCompetitionQuestionSponsorCreatedBy { get; set; }
        public virtual ICollection<CulturalCompetitionQuestion> CulturalCompetitionQuestionCreatedBy { get; set; }
        public virtual ICollection<CulturalCompetitionQuestion> CulturalCompetitionQuestionUpdatedBy { get; set; }
        public virtual ICollection<TouristVisit> TouristVisitCreatedBy { get; set; }
        public virtual ICollection<TouristVisit> TouristVisitUpdatedBy { get; set; }

        public virtual ICollection<Advertisement> AdvertisementCreatedByUser { get; set; }
        public virtual ICollection<Advertisement> AdvertisementUpdatedByUser { get; set; }

    }
}
