using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.Queue;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Serializers;
using Standard.Framework.Seedworks.Abstraction.Events;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Concrete.Queue
{
    public class Consumer : Broker, IConsumer
    {
        public Consumer(IOptions<BrokerOptions> brokerOptions, ConnectionFactory factory, IConnection connection, IModel channel) : base(brokerOptions, factory, connection, channel)
        {
            //Init();
        }

        public async Task SubscribeAsync<TRequestEvent, TIntegrationEventHandler>(IComponentContext context, 
                                                                            Action<ContainerBuilder, IConfiguration> configureScope, 
                                                                            QueueOptions options = null) where TRequestEvent : IntegrationEvent
                                                                                                         where TIntegrationEventHandler : IIntegrationEventHandler<TRequestEvent>
        {
            Tuple<string, EventingBasicConsumer> channelData = BuildChannel(options);
            ILifetimeScope rootScope = null;
            IConfiguration configuration = rootScope.Resolve<IConfiguration>();

            channelData.Item2.Received += async (model, args) =>
            {
                TRequestEvent request = null;
                rootScope = context.Resolve<ILifetimeScope>();

                using (ILifetimeScope innerScope = rootScope.BeginLifetimeScope(Guid.NewGuid().ToString(), (config) => configureScope(config, context.Resolve<IConfiguration>())))
                {
                    MessageSerializer serializaer = new MessageSerializer();
                    request = serializaer.Deserialize<TRequestEvent>(args.Body.ToArray())[0];

                    IIntegrationEventHandler<TRequestEvent> handler = innerScope.Resolve<IIntegrationEventHandler<TRequestEvent>>();
                }
            };

            Channel.BasicConsume(channelData.Item1, true, channelData.Item2);
        }

        private Tuple<string, EventingBasicConsumer> BuildChannel(QueueOptions queueOptions = null)
        {
            if (queueOptions == null && QueueOptions == null)
                throw new ArgumentNullException("Queue options parameter cannot be null.");

            QueueOptions qo = queueOptions ?? QueueOptions;

            Channel.QueueDeclare(qo.Queue, qo.Durable, qo.Exclusive, qo.AutoDelete, null);
            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);

            return new Tuple<string, EventingBasicConsumer>(qo.Queue, consumer);
        }
    }
}
