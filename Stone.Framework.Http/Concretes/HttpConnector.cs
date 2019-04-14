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

        public async Task<T> GetAsync<T>(string uri) where T : ResponseMessage
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(uri);
            }

            return DefaultHandler<T>(response);
        }

        public async Task<T> PostAsync<T>(string uri, RequestMessage request) where T : ResponseMessage
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsync(uri, content);
            }

            return DefaultHandler<T>(response);
        }

        public T DefaultHandler<T>(HttpResponseMessage httpResponse) where T : ResponseMessage
        {
            ResponseMessage response = new ResponseMessage();

            if (!httpResponse.IsSuccessStatusCode)
                response.Messages.Add(httpResponse.Content.ToString());
            else
                response = JsonConvert.DeserializeObject<T>(httpResponse.Content.ToString());

            return (T)response;
        }
    }
}
