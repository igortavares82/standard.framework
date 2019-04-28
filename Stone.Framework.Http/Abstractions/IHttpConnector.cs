using Stone.Framework.Result.Abstractions;
using System.Threading.Tasks;

namespace Stone.Framework.Http.Abstractions
{
    public interface IHttpConnector
    {
        Task<IApplicationResult<TResponse>> GetAsync<TResponse>(string uri);
        Task<IApplicationResult<TResponse>> PostAsync<TRequest, TResponse>(string uri, TRequest request);
    }
}
