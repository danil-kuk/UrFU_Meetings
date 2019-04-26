using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface IDataBaseService<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetByFilter(Expression<Func<T, bool>> filter);
        List<T> GetListByFilter(Expression<Func<T, bool>> filter);
        int GetCount(Expression<Func<T, bool>> filter);
        void Insert(T entity);
        void Remove(T entity);
        void UpdateData();
        void Update(T entity);
    }
}
