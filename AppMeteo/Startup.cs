using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppMeteo.Startup))]
namespace AppMeteo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
