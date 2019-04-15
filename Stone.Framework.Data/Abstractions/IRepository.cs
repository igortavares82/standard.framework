using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stone.Framework.Data.Abstractions
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> expression);
        void Insert(T model);
        void Update(T model);
        void Delete(T model);
    }
}
