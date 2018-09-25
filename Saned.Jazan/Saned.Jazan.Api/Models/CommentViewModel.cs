using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saned.Jazan.Api.Models
{
    public class CommentViewModel
    {
        public string CommentText { get; set; }
        public int CommentTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public int OverallCount { get; set; }
        public int? ParentId { get; set; }
        public string PhotoUrl { get; set; }
        public string RelatedId { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CommentPeriod { get; set; }
    }
}