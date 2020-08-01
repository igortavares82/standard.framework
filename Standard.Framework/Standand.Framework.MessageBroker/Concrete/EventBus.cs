using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.Queue;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Queue;
using Standand.Framework.MessageBroker.Concrete.Factories;
using Standard.Framework.Seedworks.Abstraction.Events;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Concrete
{
    public class EventBus : IEventBus
    {
        private IComponentContext Context { get; }
        private IOptions<BrokerOptions> BrokerOptions { get; }
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; }
        private IModel Channel { get; }
        private Action<ContainerBuilder, IConfiguration> ConfigureScope { get; }

        public EventBus(IComponentContext context,
                        IOptions<BrokerOptions> options,
                        Action<ContainerBuilder, IConfiguration> configureScope)
        {
            Context = context;
            BrokerOptions = options;
            Factory = BrokerMessageConnectionFactory.CreateConnection(BrokerOptions.Value);
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            ConfigureScope = configureScope;
        }

        public async Task PublishAsync(IntegrationEvent @event, QueueOptions options)
        {
            IPublisher publisher = new Publisher(BrokerOptions, Factory, Connection, Channel);
            await publisher.PublishAsync(@event, Context, options);
        }

        public async Task SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>(QueueOptions options) where TIntegrationEvent : IntegrationEvent
                                                                                                            where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
        {
            IConsumer consumer = new Consumer(BrokerOptions, Factory, Connection, Channel);
            await consumer.SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>(Context, ConfigureScope, options);
        }
    }
}
