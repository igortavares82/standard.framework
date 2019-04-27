using Stone.Framework.Message.Concretes;
using System.Threading.Tasks;

namespace Stone.Framework.Http.Abstractions
{
    public interface IHttpConnector
    {
        Task<ApplicationResult<T>> GetAsync<T>(string uri) where T : ApplicationResult<T>;
        Task<ApplicationResult<T>> PostAsync<T>(string uri, RequestMessage request) where T : ApplicationResult<T>;
    }
}
