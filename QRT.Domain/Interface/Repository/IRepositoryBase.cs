using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.Interface.Repository
{
    public interface IRepositoryBase<T> : IDisposable where T : class
    {
        IQueryable<T> All();
        IQueryable<T> All(params string[] include);
        IQueryable<T> All(params Expression<Func<T, object>>[] includes);
        IQueryable<T> AllNoTrack(params Expression<Func<T, object>>[] includes);
        IQueryable<T> All(out int total, int index = 0, int size = 50);
        IQueryable<T> All(bool tracking);
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50);
        bool Contains(Expression<Func<T, bool>> predicate);
        T Find(params object[] keys);
        T Find(Expression<Func<T, bool>> predicate);
        T Create(T t);
        int Create(IEnumerable<T> newEnume);
        int Delete(T t);
        int Delete(Expression<Func<T, bool>> predicate);
        int Update(T t);
        int Count { get; }
        int GetMaxID(Expression<Func<T, int?>> predicate);
        IEnumerable<T> SqlQuery<T>(string sql, object parameters) where T : class, new();
    }
}
