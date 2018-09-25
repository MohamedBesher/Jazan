using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Saned.Jazan.Admin.Models
{
    public class NewsImagesModel
    {
        public int ImageId { get; set; }
        [Required]
        public string ImagePath { get; set; }  
        public bool IsDefault { get; set; }
        public int NewsId { get; set; }
    }
}