using Stone.Framework.Result.Abstractions;
using System.Collections.Generic;
using System.Net;

namespace Stone.Framework.Result.Concretes
{
    public class ApplicationResult<T> : IApplicationResult<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

        public void SetStatusCode(HttpStatusCode statusCode) => StatusCode = statusCode;
        
    }
}
