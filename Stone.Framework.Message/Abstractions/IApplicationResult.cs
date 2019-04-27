using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Stone.Framework.Result.Abstractions
{
    public interface IApplicationResult<T>
    {
        T Data { get; set; }
        HttpStatusCode StatusCode { get; set; }
        List<string> Messages { get; set; }

        void SetStatusCode(HttpStatusCode statusCode);
    }
}
