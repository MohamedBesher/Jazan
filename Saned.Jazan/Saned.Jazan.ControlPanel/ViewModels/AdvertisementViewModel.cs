using System.Collections.Generic;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class AdvertisementViewModel
    {
        public IEnumerable<Advertisement> Advertisements { get; set; }
        public int Count { get; set; }
        public string SearchTerm { get; set; }
    }
}