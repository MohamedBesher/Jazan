using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Saned.Jazan.Admin.Startup))]
namespace Saned.Jazan.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
