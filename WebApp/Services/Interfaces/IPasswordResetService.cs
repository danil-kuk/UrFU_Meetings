using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Services.Interfaces
{
    public interface IPasswordResetService
    {
        PasswordReset GetByFilter(Expression<Func<PasswordReset, bool>> filter);
        void Insert(PasswordReset entity);
        void Delete(PasswordReset entity);
    }
}
