using AuthMVC.Core;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(AuthMVC.Startup))]
namespace AuthMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new DataModule(app));

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();

            //REPLACE THE MVC DEPENDENCY RESOLVER WITH AUTOFAC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            //Register With OWIN
            ConfigureAuth(app);
        }
    }
}
