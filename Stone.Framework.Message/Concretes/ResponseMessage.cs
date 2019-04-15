using System;
using System.Collections.Generic;
using System.Net;
using Stone.Framework.Message.Abstractions;

namespace Stone.Framework.Message.Concretes
{
    public class ResponseMessage<T> : BaseMessage
    {
        public ResponseMessage() { }

        public ResponseMessage(Guid messageId)
        {
            MessageId = messageId;
        }

        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
