using System.Collections.Generic;
using Stone.Framework.Message.Abstractions;

namespace Stone.Framework.Message.Concretes
{
    public class ResponseMessage : BaseMessage
    {
        public List<string> Messages { get; set; } = new List<string>();
    }
}
