using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Stone.Framework.Data.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> Get(Func<T, bool> predicate);
        Task Insert(T model);
        Task Update(T model);
        Task Delete(T model);
    }
}
