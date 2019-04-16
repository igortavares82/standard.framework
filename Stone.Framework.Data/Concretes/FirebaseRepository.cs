using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Options;
using Stone.Framework.Data.Abstractions;
using Stone.Framework.Data.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone.Framework.Data.Concretes
{
    public class FirebaseRepository<T> : IRepository<T> where T : class
    {
        //https://github.com/step-up-labs/firebase-authentication-dotnet

        protected FirebaseClient Firebase { get; }
        private IOptions<FirebaseClientOptions> ClientOptions { get; }

        public FirebaseRepository(IOptions<FirebaseClientOptions> clientOptions, FirebaseOptions firebaseOptions = null)
        {
            ClientOptions = clientOptions;
            Firebase = new FirebaseClient(clientOptions.Value.Uri, firebaseOptions);
        }

        public async Task Delete(T model)
        {
            await Firebase.Child(ClientOptions.Value.Child).DeleteAsync();
        }

        public async Task<IEnumerable<T>> Get(Func<T, bool> predicate)
        {
            Func<FirebaseObject<T>, bool> firebasePredicate = (firebaseObject) => predicate(firebaseObject.Object);
            IReadOnlyCollection<FirebaseObject<T>> collection = await Firebase.Child(ClientOptions.Value.Child).OnceAsync<T>();

            return collection.Where(firebasePredicate).Select(it => it.Object);
        }

        public async Task Insert(T model)
        {
            await Firebase.Child(ClientOptions.Value.Child).PostAsync(model);
        }

        public async Task Update(T model)
        {
            await Firebase.Child(ClientOptions.Value.Child).PutAsync(model);
        }
    }
}
