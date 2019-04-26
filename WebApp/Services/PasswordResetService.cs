using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IDataBaseService<PasswordReset> _userDataBase;

        public PasswordResetService(IDataBaseService<PasswordReset> userDataBase)
        {
            _userDataBase = userDataBase;
        }

        public void Delete(PasswordReset entity)
        {
            _userDataBase.Remove(entity);
        }

        public PasswordReset GetByFilter(Expression<Func<PasswordReset, bool>> filter)
        {
            return _userDataBase.GetByFilter(filter);
        }

        public void Insert(PasswordReset entity)
        {
            _userDataBase.Insert(entity);
        }
    }
}
