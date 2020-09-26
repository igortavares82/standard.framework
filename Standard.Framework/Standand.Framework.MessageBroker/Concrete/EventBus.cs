using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.Queue;
using Standand.Framework.MessageBroker.Abstraction.RemoteProcedureCall;
using Standand.Framework.MessageBroker.Concrete.Factories;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Queue;
using Standand.Framework.MessageBroker.Concrete.RemoteProcedureCall;
using Standard.Framework.Seedworks.Abstraction.Events;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Collections.Generic;
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
        private Dictionary<Type, Broker> Brokers { get; } = new Dictionary<Type, Broker>();

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
            IPublisher publisher = (IPublisher)ManageBrokers<IPublisher>();
            await publisher.PublishAsync(@event, Context, options);
        }

        public async Task SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>(QueueOptions options) where TIntegrationEvent : IntegrationEvent
                                                                                                            where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>
        {
            IConsumer consumer = (IConsumer)ManageBrokers<IConsumer>();
            await consumer.SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>(Context, ConfigureScope, options);
        }

        public async Task SubscribeAsync<TRequestEvent, TResponseEvent, TIntegrationEventHandler>(QueueOptions options) where TRequestEvent : IntegrationEvent
                                                                                                                        where TResponseEvent : IntegrationEvent
                                                                                                                        where TIntegrationEventHandler : IIntegrationEventHandler<TRequestEvent, TResponseEvent>
        {
            IServer server = (IServer)ManageBrokers<IServer>();
            await server.CallHandlerAsync<TRequestEvent, TResponseEvent, TIntegrationEventHandler>(Context, ConfigureScope, options);
        }

        public async Task<TResponseEvent> CallAsync<TRequestEvent, TResponseEvent>(TRequestEvent request, QueueOptions options) where TRequestEvent : IntegrationEvent
                                                                                                                                where TResponseEvent : IntegrationEvent
        {
            IClient client = (IClient)ManageBrokers<IClient>();
            return await client.CallAsync<TRequestEvent, TResponseEvent>(request, Context, options);
        }

        private Broker ManageBrokers<TBroker>()
        {
            Broker broker = null;

            if (Brokers.ContainsKey(typeof(TBroker)))
                broker = Brokers[typeof(TBroker)];
            else
            {
                if (typeof(TBroker) == typeof(IPublisher))
                { 
                    broker = new Publisher(BrokerOptions, Factory, Connection, Channel);
                    Brokers.Add(typeof(TBroker), broker);
                }

                if (typeof(TBroker) == typeof(IConsumer))
                {
                    broker = new Consumer(BrokerOptions, Factory, Connection, Channel);
                    Brokers.Add(typeof(TBroker), broker);
                }

                if (typeof(TBroker) == typeof(IClient)) 
                {
                    broker = new Client(BrokerOptions, Factory, Connection, Channel);
                    Brokers.Add(typeof(TBroker), broker);
                }

                if (typeof(TBroker) == typeof(IServer))
                {
                    broker = new Server(BrokerOptions, Factory, Connection, Channel);
                    Brokers.Add(typeof(TBroker), broker);
                }
            }

            return broker;
        }
    }
}
