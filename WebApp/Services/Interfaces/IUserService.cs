using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Services
{
    public interface IUserService
    {
        User GetById(int id);
        User GetByFilter(Expression<Func<User, bool>> filter);
        void InsertUser(User user);
        void UpdateUserData();
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
