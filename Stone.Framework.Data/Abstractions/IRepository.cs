using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stone.Framework.Data.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
    }
}
