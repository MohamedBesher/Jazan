using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saned.Jazan.ControlPanel.Startup))]
namespace Saned.Jazan.ControlPanel
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
