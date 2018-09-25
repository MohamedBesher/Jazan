using System.Collections.Generic;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class NewsViewModel
    {
        public IEnumerable<News> News { get; set; }
        public int Count { get; set; }
        public string SearchTerm { get; set; }
    }
}