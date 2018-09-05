using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthMVC.Models.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AuthMVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
    UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual UserProfile Profile { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
    : base("DefaultConnection")
        {
            // Database.SetInitializer<ApplicationDbContext>(new MyContextInitializer());
        }
        public virtual DbSet<UserProfile> Profiles { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductInfo> ProductInfoes { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    class MyContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            IList<CustomRole> defaultRoles = new List<CustomRole>();

            defaultRoles.Add(new CustomRole() { Id=1, Name= "Administrator" });
            defaultRoles.Add(new CustomRole() { Id = 2, Name = "Moderator" });
            defaultRoles.Add(new CustomRole() { Id = 3, Name = "User" });

            foreach (var item in defaultRoles)
            {
                db.Roles.Add(item);
            }


            for (int i = 1; i < 4; i++)
            {
                CustomUserRole role = new CustomUserRole { RoleId = 3 };
                var profile = new UserProfile { Address = "Ukraine",  BirthDay = Convert.ToDateTime(DateTime.Now)};
                var user = new ApplicationUser { UserName = "user" + i + "@ukr.net",
                    Email = "user" + i + "@ukr.net",
                    PhoneNumber = "0985652211",
                    Profile = profile,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    PasswordHash = "AOfCM/mu6RJPo0BpZ4AHjwULyz/u2jr4l6UtqIPCVKoMaAyED5Jaw1qhDHGCmVEdJA==",
                    SecurityStamp = "b8496e15-bacf-427c-9aa3-f62c128e0f43"
                };
                user.Roles.Add(role);
                db.Users.Add(user);
            }
            for (int i = 1; i < 4; i++)
            {
                CustomUserRole role = new CustomUserRole { RoleId = 2 };
                var profile = new UserProfile { Address = "USA", BirthDay = Convert.ToDateTime(DateTime.Now) };
                var user = new ApplicationUser
                {
                    UserName = "moderator" + i + "@ukr.net",
                    Email = "moderator" + i + "@ukr.net",
                    PhoneNumber = "0985652233",
                    Profile = profile,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    PasswordHash = "AOfCM/mu6RJPo0BpZ4AHjwULyz/u2jr4l6UtqIPCVKoMaAyED5Jaw1qhDHGCmVEdJA==",
                    SecurityStamp = "b8496e15-bacf-427c-9aa3-f62c128e0f43"
                };
                user.Roles.Add(role);
                db.Users.Add(user);
            }
            for (int i = 1; i < 3; i++)
            {
                CustomUserRole role = new CustomUserRole { RoleId = 1 };
                var profile = new UserProfile { Address = "Germany", BirthDay = Convert.ToDateTime(DateTime.Now) };
                var user = new ApplicationUser
                {
                    UserName = "admin" + i + "@ukr.net",
                    Email = "admin" + i + "@ukr.net",
                    PhoneNumber = "0985652233",
                    Profile = profile,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEndDateUtc = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    PasswordHash = "AOfCM/mu6RJPo0BpZ4AHjwULyz/u2jr4l6UtqIPCVKoMaAyED5Jaw1qhDHGCmVEdJA==",
                    SecurityStamp = "b8496e15-bacf-427c-9aa3-f62c128e0f43"
                };
                user.Roles.Add(role);
                db.Users.Add(user);
            }

            base.Seed(db);
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}