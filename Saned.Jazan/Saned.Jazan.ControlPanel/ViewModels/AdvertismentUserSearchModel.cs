namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class AdvertismentUserSearchModel:Pager
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public int CategoryId { get; set; }
        public int PackageId { get; set; }

        public bool? Approved
        {
            get
            {
                if (!string.IsNullOrEmpty(IsApproved))
                    return IsApproved == "1";

                else
                    return null;
            }
        }

        public string IsApproved { get; set; }
        public string UserId { get; set; }
    }

    public class TouristVisitsUserSearchModel:Pager
    {
        public int Id { get; set; }
        public string Keyword { get; set; }
        public string UserId { get; set; }
        public string CityName { get; set; }
        public bool? Approved
        {
            get
            {
                if (!string.IsNullOrEmpty(IsApproved))
                    return IsApproved == "1";

                else
                    return null;
            }
        }

        public string IsApproved { get; set; }
    }

}