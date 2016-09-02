using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bellcast.Startup))]
namespace bellcast
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
