using AuthMVC.DAL.Abstract;
using AuthMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        public bool CanManage(int curuser, int user)
        {
            var manager = GetRoleId(curuser);
          

            var delete = GetRoleId(user);


            if (delete <= manager)
                return false;
            return true;
        }

        public int GetRoleId(int userId)
        {
            return _context.Users.Find(userId).Roles.FirstOrDefault().RoleId;
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

        public UserEditModel GetUser(int id)
        {
            ApplicationUser user = _context.Users.Find(id);


            UserEditModel model = new UserEditModel
            {
                Id = id,
                Role = GetRoleId(id),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Profile.Address,
                Photo = user.Profile.Photo,
                BirthDay = user.Profile.BirthDay.ToString("yyyy-MM-dd")        
            };

            return model;
        }

        public bool ChangeUser(UserEditModel model)
        {
            ApplicationUser usertochange = _context.Users.Find(model.Id);
            string path = "";
            string filename = "";
            string extension = "";


            if (model.Photo != null)
            {
                int startIndex = model.Photo.IndexOf("/") + 1;
                int lastIndex = model.Photo.IndexOf(";");
                extension = "." + model.Photo.Substring(startIndex, lastIndex - startIndex);
                filename = model.Email + "_avatar";
                path = @"/Content/SaveAvatars/" + filename + extension;
                if(usertochange.Profile.Photo!=null)
                try
                {
                   File.Delete(usertochange.Profile.Photo);
                }
                catch (Exception)
                { }
                usertochange.Profile.Photo = path;
            }
            

            usertochange.Email = model.Email;
            usertochange.UserName = model.Email;
            usertochange.PhoneNumber = model.PhoneNumber;
            usertochange.Profile.Address = model.Address;
            usertochange.Profile.BirthDay = Convert.ToDateTime(model.BirthDay);
            

            CustomUserRole role = new CustomUserRole { RoleId = model.Role};

            usertochange.Roles.Clear();
            usertochange.Roles.Add(role);
            try
            {
                if (model.Photo != null)
                {
                    var fs = new BinaryWriter(new FileStream(HttpContext.Current.Server.MapPath("~/Content/SaveAvatars/" + filename + extension), FileMode.Create, FileAccess.Write));
                    string base64img = model.Photo.Split(',')[1];
                    byte[] buf = Convert.FromBase64String(base64img);
                    fs.Write(buf);
                    fs.Close();
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {

                return false;
            }


            return true;
        }
    }
}