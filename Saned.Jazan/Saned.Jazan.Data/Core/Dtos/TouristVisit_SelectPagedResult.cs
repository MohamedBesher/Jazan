namespace Saned.Jazan.Data.Core.Dtos
{
    public class TouristVisit_SelectPagedResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int OverallCount { get; set; }
        public int Rating { get; set; }
        public int Views { get; set; }
        public string Description { get; set; }
        public bool IsApproved { get; set; }
    }
}
