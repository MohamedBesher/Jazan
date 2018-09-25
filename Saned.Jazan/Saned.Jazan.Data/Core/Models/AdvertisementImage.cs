namespace Saned.Jazan.Data.Core.Models
{
    public class AdvertisementImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }
    }
}
