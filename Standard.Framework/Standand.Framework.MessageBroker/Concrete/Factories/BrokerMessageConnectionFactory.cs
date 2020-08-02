using RabbitMQ.Client;
using Standand.Framework.MessageBroker.Concrete.Options;

namespace Standand.Framework.MessageBroker.Concrete.Factories
{
    internal class BrokerMessageConnectionFactory
    {
        public static ConnectionFactory CreateConnection(BrokerOptions options)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = options.HostName,
                Port = AmqpTcpEndpoint.UseDefaultPort,
                VirtualHost = ConnectionFactory.DefaultVHost
            };

            if (!string.IsNullOrEmpty(options.UserName))
                factory.UserName = options.UserName;

            if (!string.IsNullOrEmpty(options.Password))
                factory.Password = options.Password;

            return factory;
        }
    }
}

