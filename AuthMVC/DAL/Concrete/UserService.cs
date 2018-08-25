using AuthMVC.DAL.Abstract;
using AuthMVC.Models;
using System;
using System.Collections.Generic;
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

        public int GetCountUsers()
        {
            return _context.Users.Count();
        }
    }
}