using Newtonsoft.Json;
using Stone.Framework.Http.Abstractions;
using Stone.Framework.Result.Abstractions;
using Stone.Framework.Result.Concretes;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stone.Framework.Http.Concretes
{
    public class HttpConnector : IHttpConnector
    {
        public Uri Address { get; private set; }

        public void SetAddress(string address) => Address = new Uri(address);

        public async Task<IApplicationResult<TResponse>> GetAsync<TResponse>(string uri)
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient() { BaseAddress = Address })
            {
                response = await client.GetAsync(uri);
            }

            return DefaultHandler<TResponse>(response);
        }

        public async Task<IApplicationResult<TResponse>> PostAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient() { BaseAddress = Address })
            {
                response = await client.PostAsync(uri, content);
            }

            return DefaultHandler<TResponse>(response);
        }

        private IApplicationResult<TResponse> DefaultHandler<TResponse>(HttpResponseMessage httpResponse)
        {
            IApplicationResult<TResponse> response = new ApplicationResult<TResponse>();

            if (!httpResponse.IsSuccessStatusCode)
                response.Messages.Add(httpResponse.Content.ToString());
            else
                response.Data = JsonConvert.DeserializeObject<TResponse>(httpResponse.Content.ToString());

            response.StatusCode = httpResponse.StatusCode;

            return response;
        }
    }
}
