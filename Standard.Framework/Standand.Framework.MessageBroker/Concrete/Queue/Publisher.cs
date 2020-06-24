using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.Queue;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standard.Framework.Seedworks.Abstraction.Events;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Concrete.Queue
{
    public class Publisher : Broker, IPublisher
    {
        public Publisher(IOptions<BrokerOptions> brokerOptions, ConnectionFactory factory, IConnection connection, IModel channel) : base(brokerOptions, factory, connection, channel)
        {
            Init();
        }

        public Task SubscribeAsync<TRequestEvent, TIntegrationEventHandler>(IComponentContext context, 
                                                                            Action<ContainerBuilder, IConfiguration> configureScope, 
                                                                            QueueOptions options = null) where TRequestEvent : IntegrationEvent
                                                                                                         where TIntegrationEventHandler : IIntegrationEventHandler<TRequestEvent>
        {
            throw new NotImplementedException();
        }
    }
}
