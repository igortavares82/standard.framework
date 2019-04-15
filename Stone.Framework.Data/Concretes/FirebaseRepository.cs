using Firebase.Database;
using Microsoft.Extensions.DependencyInjection;
using Stone.Framework.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Stone.Framework.Data.Concretes
{
    public class FirebaseRepository<T> : IRepository<T> where T : class
    {
        protected FirebaseClient Firebase { get; }

        public FirebaseRepository(string uri, FirebaseOptions options = null)
        {
            Firebase = new FirebaseClient("", options);
        }

        public void Delete(T model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Insert(T model)
        {
            throw new NotImplementedException();
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
