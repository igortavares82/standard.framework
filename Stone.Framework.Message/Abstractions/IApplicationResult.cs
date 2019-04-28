using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace Stone.Framework.Result.Abstractions
{
    public interface IApplicationResult<T> : IActionResult
    {
        T Data { get; set; }
        HttpStatusCode StatusCode { get; set; }
        List<string> Messages { get; set; }

        void SetStatusCode(HttpStatusCode statusCode);
    }
}
