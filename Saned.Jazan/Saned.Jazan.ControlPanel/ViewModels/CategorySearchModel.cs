namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class CategorySearchModel
    {
        public int Page { get; set; } = 1;
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Keyword { get; set; }
    }
}