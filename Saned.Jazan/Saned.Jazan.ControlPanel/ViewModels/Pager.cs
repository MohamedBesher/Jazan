using Saned.Jazan.ControlPanel.Properties;

namespace Saned.Jazan.ControlPanel.ViewModels
{
    public class Pager
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = Settings.Default.PageSize;

    }
}