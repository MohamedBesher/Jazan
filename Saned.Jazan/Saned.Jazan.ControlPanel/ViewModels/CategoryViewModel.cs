using System.Collections.Generic;
using Saned.Jazan.Data.Core.Dtos;
using Saned.Jazan.Data.Core.Models;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class CategoryViewModel
    {
        public string ParentName { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int Count { get; set; }
        public string SearchTerm { get; set; }
    }
    public class CategoryViewModel2
    {
        public string CategoryId { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }
        public string ParentName { get; set; }
        public IEnumerable<CategoryView> Categories { get; set; }
        public int Count { get; set; }
        public string SearchTerm { get; set; }
    }

}