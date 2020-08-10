using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.RemoteProcedureCall;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Serializers;
using Standard.Framework.Seedworks.Abstraction.Events;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Concrete.RemoteProcedureCall
{
    public class Server : Broker, IServer
    {
        public Server(IOptions<BrokerOptions> brokerOptions, ConnectionFactory factory, IConnection connection, IModel channel) : base(brokerOptions, factory, connection, channel) { }

        public async Task CallHandlerAsync<TRequest, TResponse, TIntegrationEventHandler>(IComponentContext context, 
                                                                                    Action<ContainerBuilder, IConfiguration> configureScope, 
                                                                                    QueueOptions options) where TRequest : IntegrationEvent
                                                                                                          where TResponse : IntegrationEvent
        {
            EventingBasicConsumer consumer = BuildChannel(options);

            consumer.Received += async (model, args) =>
            {
                TRequest request = null;
                ILifetimeScope rootScope = context.Resolve<ILifetimeScope>();

                using (ILifetimeScope innerScope = rootScope.BeginLifetimeScope(Guid.NewGuid().ToString(), (config) => configureScope(config, context.Resolve<IConfiguration>())))
                {
                    MessageSerializer serializaer = new MessageSerializer();
                    IBasicProperties props = args.BasicProperties;
                    IBasicProperties replyProps = Channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    request = Activator.CreateInstance<TRequest>();

                    if (args.Body.Length > 0)
                        request = serializaer.Deserialize<TRequest>(args.Body.ToArray())[0];

                    IIntegrationEventHandler<TRequest, TResponse> handler = innerScope.Resolve<IIntegrationEventHandler<TRequest, TResponse>>();
                    TResponse response = await handler.Handle(request);

                    Channel.BasicPublish("", props.ReplyTo, replyProps, serializaer.Serialize(response)[0]);
                    Channel.BasicAck(args.DeliveryTag, false);
                }
            };
        }

        private EventingBasicConsumer BuildChannel(QueueOptions options)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);

            Channel.QueueDeclare(options.Queue, options.Durable, options.Exclusive, options.AutoDelete, null);
            Channel.BasicQos(0, 1, false);
            Channel.BasicConsume(options.Queue, false, consumer);

            return consumer;
        }
    }
}
