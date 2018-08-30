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
        bool CanManage(int curuser, int deluser);
        List<CustomRole> GetRoles();
        int GetRoleId(int userId);
        UserEditModel GetUser(int id);

        bool ChangeUser(UserEditModel model);
    }
}