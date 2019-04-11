using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataBase<User> _databaseUser;

        public UserService(IUserDataBase<User> databaseUser)
        {
            _databaseUser = databaseUser;
        }

        public void DeleteUser(User user)
        {
            _databaseUser.Remove(user);
        }

        public User GetByFilter(Expression<Func<User, bool>> filter)
        {
            return _databaseUser.GetByFilter(filter);
        }

        public User GetById(int id)
        {
            return _databaseUser.GetById(id);
        }

        public void InsertUser(User user)
        {
            _databaseUser.Insert(user);
        }
        public void UpdateUser(User user)
        {
            _databaseUser.Update(user);
        }

        public void UpdateUserData()
        {
            _databaseUser.UpdateUserData();
        }
    }
}
