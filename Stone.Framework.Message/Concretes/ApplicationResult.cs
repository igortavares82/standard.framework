using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stone.Framework.Result.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stone.Framework.Result.Concretes
{
    public class ApplicationResult<T> : IApplicationResult<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

        public void SetStatusCode(HttpStatusCode statusCode) => StatusCode = statusCode;

        public async Task ExecuteResultAsync(ActionContext context)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(this));

            context.HttpContext.Response.Headers.Add("content-type", "application/json");
            context.HttpContext.Response.StatusCode = (int)this.StatusCode;
            await content.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}
