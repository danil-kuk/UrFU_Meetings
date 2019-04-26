using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;
using WebApp.Services.Interfaces;

namespace WebApp.Services
{
    public class ActivationService : IActivationService
    {
        private readonly IDataBaseService<EmailValid> _userDataBase;

        public ActivationService(IDataBaseService<EmailValid> userDataBase)
        {
            _userDataBase = userDataBase;
        }

        public void Delete(EmailValid entity)
        {
            _userDataBase.Remove(entity);
        }

        public EmailValid GetByFilter(Expression<Func<EmailValid, bool>> filter)
        {
            return _userDataBase.GetByFilter(filter);
        }

        public void Insert(EmailValid entity)
        {
            _userDataBase.Insert(entity);
        }
    }
}
