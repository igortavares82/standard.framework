namespace Standand.Framework.MessageBroker.Concrete.Options
{
    public class ChannelOptions
    {
        public string Exchange { get; set; }
        public string Type { get; set; }
        public bool Durable { get; set; }
        public string RoutingKey { get; set; }
    }
}
