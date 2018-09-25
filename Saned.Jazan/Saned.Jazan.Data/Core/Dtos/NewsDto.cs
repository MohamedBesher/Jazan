using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Jazan.Data.Core.Dtos
{
    public class SingleNewsDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public List<string> Images { get; set; }
        public int DefaultIndex { get; set; }
        public DateTime PublishingDate { get; set; }
    }

    public class SingleNews
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime PublishingDate { get; set; }

    }
    public class NewsImageDto
    {
        public int ImageId { get; set; }
        public bool IsDefault { get; set; }
        public string ImagePath { get; set; }
    }
    public class ViewNewsDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string DefaultImage { get; set; }
        public DateTime PublishingDate { get; set; }
    }
    public class InsertNewsDto
    {
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public List<string> ImagesBase64s { get; set; }
        [Required]
        public List<string> ImageExtentions { get; set; }
        [Required]
        public int DefaultImageIndex  { get; set; }
    }

    public class UpdateNewsDto
    {
        [Required]
        public int NewsId { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public List<int> ImagesToDelete { get; set; }
        public List<string> NewImagesBase64s { get; set; }
        public List<string> NewImageExtentions { get; set; }
        [Required]
        public int DefaultImageIndex { get; set; }  // if -1 then it is not in the new Image list .
    }

    public class NewsRequestDto
    {
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

        [MaxLength(8,ErrorMessage ="You may Provide date in wrong pattern pleas provide date in the pattern of YYYYMMDD")]
        public string DateFilter { get; set; }

        [MaxLength(300)]
        public string TitleFilter { get; set; }
        public string DetailFilter { get; set; }
    }
}
