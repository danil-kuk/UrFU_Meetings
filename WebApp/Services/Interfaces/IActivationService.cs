using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Services.Interfaces
{
    public interface IActivationService
    {
        EmailValid GetByFilter(Expression<Func<EmailValid, bool>> filter);
        void Insert(EmailValid entity);
        void Delete(EmailValid entity);
    }
}
