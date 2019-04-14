using System;

namespace Stone.Framework.Message.Abstractions
{
    public abstract class BaseMessage
    {
        public Guid MessageId { get; set; }
    }
}
