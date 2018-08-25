using AuthMVC.DAL.Abstract;
using AuthMVC.DAL.Concrete;
using AuthMVC.Models;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthMVC.Core
{
    public class DataModule: Module
    {
        private readonly IAppBuilder _app;
        public DataModule(IAppBuilder app)
        {
            _app = app;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ApplicationDbContext()).AsSelf().InstancePerRequest();
            builder.RegisterType<CustomUserStore>()
                .As<IUserStore<ApplicationUser, int>>()
                .InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<CustomRoleStore>()
                .As<IRoleStore<CustomRole, int>>()
                .InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication)
                .InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => _app.GetDataProtectionProvider())
                .InstancePerRequest();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();

            base.Load(builder);
        }
    }
}