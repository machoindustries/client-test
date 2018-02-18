using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(xcentium.Startup))]
namespace xcentium
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
