using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface IUserDataBase<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetByFilter(Expression<Func<T, bool>> filter);
        List<T> GetListByFilter(Expression<Func<T, bool>> filter);
        int GetCount(Expression<Func<T, bool>> filter);
        void Insert(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
