using Firebase.Database;
using System.Threading.Tasks;

namespace Stone.Framework.Data.Options
{
    public class FirebaseClientOptions : FirebaseOptions
    {
        public string Uri { get; set; }
        public string Child { get; set; }
        public string AuthToken { get; set; }

        public void SetAuthToken() => base.AuthTokenAsyncFactory = () => Task.FromResult(AuthToken);
    }
}
