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

        public async Task<IApplicationResult<TResponse>> GetAsync<TResponse>(string uri, bool isDefaultResponse = true)
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.GetAsync(new Uri(string.Concat(Address, uri)));
            }

            return DefaultHandler<TResponse>(response, isDefaultResponse);
        }

        public async Task<IApplicationResult<TResponse>> PostAsync<TRequest, TResponse>(string uri, TRequest request, bool isDefaultResponse = true)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsync(new Uri(string.Concat(Address, uri)), content);
            }

            return DefaultHandler<TResponse>(response, isDefaultResponse);
        }

        private IApplicationResult<TResponse> DefaultHandler<TResponse>(HttpResponseMessage httpResponse, bool isDefaultResponse)
        {
            IApplicationResult<TResponse> response = new ApplicationResult<TResponse>();

            if (!httpResponse.IsSuccessStatusCode)
                response.Messages.Add(httpResponse.Content.ReadAsStringAsync().Result);
            else if (isDefaultResponse)
            {
                string json = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ApplicationResult<TResponse>>(json);
            }
            else
            {
                string json = httpResponse.Content.ReadAsStringAsync().Result;
                response.Data = JsonConvert.DeserializeObject<TResponse>(json);
            }

            response.StatusCode = httpResponse.StatusCode;

            return response;
        }
    }
}
