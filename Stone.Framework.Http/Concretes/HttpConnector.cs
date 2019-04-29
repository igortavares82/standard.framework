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
        public string Address { get; private set; }
        public void SetAddress(string address) => Address = address;

        public async Task<IApplicationResult<TResponse>> GetAsync<TResponse>(string uri)
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(new Uri(string.Concat(Address, uri)));
            }

            return DefaultHandler<TResponse>(response);
        }

        public async Task<IApplicationResult<TResponse>> PostAsync<TRequest, TResponse>(string uri, TRequest request)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsync(new Uri(string.Concat(Address, uri)), content);
            }

            return DefaultHandler<TResponse>(response);
        }

        private IApplicationResult<TResponse> DefaultHandler<TResponse>(HttpResponseMessage httpResponse)
        {
            IApplicationResult<TResponse> response = new ApplicationResult<TResponse>();

            if (!httpResponse.IsSuccessStatusCode)
                response.Messages.Add(httpResponse.Content.ToString());
            else
            {
                string json = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<IApplicationResult<TResponse>>(json);
            }

            response.StatusCode = httpResponse.StatusCode;

            return response;
        }
    }
}
