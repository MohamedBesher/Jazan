using System.ComponentModel.DataAnnotations;

namespace Saned.Jazan.Data.Core.Models
{
    public class NewsImage
    { 
       
        public int ImageId { get; set; }
        public string ImagePath { get; set; }  //Guid.NewGuid().ToString("N")
        public bool IsDefault { get; set; }
        public int NewsId { get; set; } 
        public virtual News News { get; set; }
    }
}
