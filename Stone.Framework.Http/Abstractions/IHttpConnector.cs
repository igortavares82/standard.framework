using Stone.Framework.Result.Abstractions;
using System.Threading.Tasks;

namespace Stone.Framework.Http.Abstractions
{
    public interface IHttpConnector
    {
        void SetAddress(string address);
        Task<IApplicationResult<TResponse>> GetAsync<TResponse>(string uri, bool isDefaultResponse = true);
        Task<IApplicationResult<TResponse>> PostAsync<TRequest, TResponse>(string uri, TRequest request, bool isDefaultResponse = true);
    }
}
