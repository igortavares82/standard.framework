﻿namespace Standand.Framework.MessageBroker.Concrete.Options
{
    public class QueueOptions
    {
        public string Queue { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
    }
}
