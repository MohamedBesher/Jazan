using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class CategoryDtos
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CategoryView
    {
        public int CategoryId { get; set; }
        public string CategoryNameAr { get; set; }
        public string CategoryImage { get; set; }
        public string ParentName { get; set; }
        public int? ParentId { get; set; }
    }

    public class CategoriesRequest
    {
        public int LanguageId { get; set; } // 0 or 1 .

        private int pageNumaber = 1;
        public int PageNumber
        {
            get
            {
                if (pageNumaber != 0)
                {
                    return pageNumaber;
                }
                return 1;
            }
            set { pageNumaber = value; }
        }

        private int pageSize = 10;
        public int PageSize
        {
            get
            {
                if (pageSize != 0)
                    return pageSize;
                return 10;
            }
            set { pageSize = value; }
        }

        [MaxLength(400)]
        public string NameFilter { get; set; }
    }

    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string CategoryImageBase64 { get; set; }
        public string ImageExtention { get; set; }
        public int ParentId { get; set; } // default 0 
        public int LanguageId { get; set; } // 0 or 1 .

    }



    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string NewCategoryImageBase64 { get; set; }
        public string ImageExt { get; set; }
        public int Status { get; set; }
        public int LanguageId { get; set; } // 0 or 1 .
    }
}
