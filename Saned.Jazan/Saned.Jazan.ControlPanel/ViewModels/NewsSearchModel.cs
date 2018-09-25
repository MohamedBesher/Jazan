using System;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class NewsSearchModel : Pager
    {
        public string SearchTerm { get; set; }
        public int Id { get; set; }
        public DateTime? PublishingDate { get; set; }
    }
}