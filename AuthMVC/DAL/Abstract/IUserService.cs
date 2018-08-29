using AuthMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthMVC.DAL.Abstract
{
    public interface IUserService
    {
        int GetCountUsers();
        int AddRole(string name);
        List<UserViewModel> GetUsers();
        void DeleteUser(int id);
        bool CanDelete(int curuser, int deluser);
        List<CustomRole> GetRoles();
    }
}