using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AuthMVC.Startup))]
namespace AuthMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
