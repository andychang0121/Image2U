using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Image2U.Web.Startup))]
namespace Image2U.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
