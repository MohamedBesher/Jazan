using System;
using System.Collections.Generic;

namespace Saned.Jazan.Data.Core.Models
{
    public sealed class News
    {
        public News()
        {
            NewsImages = new List<NewsImage>();
            PublishingDate = new DateTime();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime PublishingDate { get; set; }
        public ICollection<NewsImage> NewsImages { get; set; }
    }
}
