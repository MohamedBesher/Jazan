using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Saned.Jazan.Data.Core.Models;
using System.ComponentModel.DataAnnotations;


namespace Saned.Jazan.Admin.Models
{
    public class NewsModels
    {
        public int Id { get; set; }
        
        [Display(Name = "Title")]
      //  [Required(ErrorMessageResourceType = typeof(General), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Details")]
        public string Details { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "PublishingDate")]
        public DateTime PublishingDate { get; set; }
        
        public virtual List<NewsImagesModel> NewsImagesModel { get; set; }


        public static implicit operator News(NewsModels model)
        {

            return new News()
            {
                Id = model.Id,
                Title = model.Title,
                Details = model.Details,
                PublishingDate = model.PublishingDate,
                NewsImages = model.NewsImagesModel != null
                    ? model.NewsImagesModel.Select(x => new NewsImage()
                    {
                        ImageId =x.ImageId,
                        ImagePath = x.ImagePath,
                        IsDefault = x.IsDefault,
                        NewsId = x.NewsId
                    })
                        .ToList()
                    : new List<NewsImage>()

            };
        }

        public static explicit operator NewsModels(News model)
        {
            return new NewsModels()
            {
                Id = model.Id,
                Title = model.Title,
                Details = model.Details,
                PublishingDate = model.PublishingDate,
                NewsImagesModel = model.NewsImages != null
                    ? model.NewsImages.Select(x => new NewsImagesModel()
                    {
                        ImageId = x.ImageId,
                        ImagePath = x.ImagePath,
                        IsDefault = x.IsDefault,
                        NewsId = x.NewsId
                    })
                        .ToList()
                    : new List<NewsImagesModel>()
            };
        }
    }
}