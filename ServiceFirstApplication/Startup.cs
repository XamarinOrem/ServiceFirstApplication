using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServiceFirstApplication.Startup))]
namespace ServiceFirstApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
