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
        private readonly IRepositoryBase<User> _repositoryUser;

        public UserService(IRepositoryBase<User> repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }

        public void DeleteUser(User user)
        {
            _repositoryUser.Remove(user);
        }

        public User GetByFilter(Expression<Func<User, bool>> filter)
        {
            return _repositoryUser.GetByFilter(filter);
        }

        public User GetById(int id)
        {
            return _repositoryUser.GetById(id);
        }

        public void InsertUser(User user)
        {
            _repositoryUser.Insert(user);
        }

        public void UpdateUser(User user)
        {
            _repositoryUser.Update(user);
        }
    }
}
