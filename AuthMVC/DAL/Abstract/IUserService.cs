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
    }
}