using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Factories;
using System;

namespace Standand.Framework.MessageBroker.Abstraction
{
    public class Broker
    {
        public BrokerOptions BrokerOptions { get; protected set; }
        public ChannelOptions ChannelOptions { get; protected set; } = new ChannelOptions() { Type = "fanout", RoutingKey = "", Durable = true, Exchange = "amq.fanout" };
        public QueueOptions QueueOptions { get; protected set; }

        public static ConnectionFactory Factory { get; protected set; }

        protected IConnection Connection { get; set; }
        protected IModel Channel { get; set; }

        public Broker(IOptions<BrokerOptions> brokerOptions, ConnectionFactory factory, IConnection connection, IModel channel)
        {
            BrokerOptions = brokerOptions.Value ?? throw new ArgumentNullException("Broker options argument cannot be null.");

            Factory = factory;
            Connection = connection;
            Channel = channel ?? Connection.CreateModel();
        }

        protected virtual void Init()
        {
            Factory = BrokerMessageConnectionFactory.CreateConnection(BrokerOptions);

            if (!string.IsNullOrEmpty(BrokerOptions.UserName))
                Factory.UserName = BrokerOptions.UserName;

            if (!string.IsNullOrEmpty(BrokerOptions.Password))
                Factory.Password = BrokerOptions.Password;

            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
        }
    }
}
