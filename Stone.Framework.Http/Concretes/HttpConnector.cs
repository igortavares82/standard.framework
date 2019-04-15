using Newtonsoft.Json;
using Stone.Framework.Http.Abstractions;
using Stone.Framework.Message.Concretes;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Framework.Http.Concretes
{
    public class HttpConnector : IHttpConnector
    {
        public string Address { get; private set; }

        public void SetAddress(string address) => Address = address;

        public async Task<ResponseMessage<T>> GetAsync<T>(string uri) where T : ResponseMessage<T>
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(uri);
            }

            return DefaultHandler<T>(response);
        }

        public async Task<ResponseMessage<T>> PostAsync<T>(string uri, RequestMessage request) where T : ResponseMessage<T>
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsync(uri, content);
            }

            return DefaultHandler<T>(response);
        }

        public ResponseMessage<T> DefaultHandler<T>(HttpResponseMessage httpResponse) where T : ResponseMessage<T>
        {
            ResponseMessage<T> response = new ResponseMessage<T>();

            if (!httpResponse.IsSuccessStatusCode)
                response.Messages.Add(httpResponse.Content.ToString());
            else
                response.Data = JsonConvert.DeserializeObject<T>(httpResponse.Content.ToString());

            response.StatusCode = httpResponse.StatusCode;

            return (ResponseMessage<T>)response;
        }
    }
}
