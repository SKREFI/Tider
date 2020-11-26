using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tider.Startup))]
namespace Tider
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
