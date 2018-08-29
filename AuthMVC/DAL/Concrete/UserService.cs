using AuthMVC.DAL.Abstract;
using AuthMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AuthMVC.DAL.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationRoleManager _roleManager;

        public UserService(ApplicationDbContext context, ApplicationRoleManager roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public int AddRole(string name)
        {
            CustomRole role = new CustomRole
            {
                Name = name
            };
            var result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
                return role.Id;
            return 0;
        }

        public bool CanDelete(int curuser, int deluser)
        {
            var manager = _context.Users.Find(curuser).Roles.FirstOrDefault().RoleId;
          

            var delete = _context.Users.Find(deluser).Roles.FirstOrDefault().RoleId;
           

            if (delete <= manager)
                return false;
            return true;

        }

        public void DeleteUser(int id)
        {
            var profile = new UserProfile { Id = id };
            _context.Entry(profile).State = EntityState.Deleted;
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);

            try { _context.SaveChanges(); }
            catch { }
        }

        public int GetCountUsers()
        {
            return _context.Users.Count();
        }

        public List<CustomRole> GetRoles()
        {
           
            return _roleManager.Roles.ToList();
        }

        public List<UserViewModel> GetUsers()
        {

            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var item in _context.Users.Include(a=>a.Roles))
            {
                var userrol = item.Roles.FirstOrDefault();
                
                string role = "";
                if (userrol != null)
                {
                    var userroleid = userrol.RoleId;
                    role = _roleManager.Roles.Where((x) => x.Id == userroleid).First().Name;
                }
                UserViewModel model = new UserViewModel
                {
                    UserID = item.Id,
                    UserLogin = item.Email,
                    Country = item.Profile.Address,
                    UserRole = role                  
                };
                users.Add(model);
            }

            return users;
        }
    }
}