namespace Standand.Framework.MessageBroker.Concrete.Options
{
    public class BrokerOptions
    {
        public string HostName { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public int? MaxDegreeOfParallelism { get; set; }
    }
}
