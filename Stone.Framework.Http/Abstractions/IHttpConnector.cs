using Stone.Framework.Message.Concretes;
using System.Threading.Tasks;

namespace Stone.Framework.Http.Abstractions
{
    public interface IHttpConnector
    {
        Task<ResponseMessage<T>> GetAsync<T>(string uri) where T : ResponseMessage<T>;
        Task<ResponseMessage<T>> PostAsync<T>(string uri, RequestMessage request) where T : ResponseMessage<T>;
    }
}
