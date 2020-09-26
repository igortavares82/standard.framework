using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.RemoteProcedureCall;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Serializers;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Concrete.RemoteProcedureCall
{
    public class Client : Broker, IClient
    {
        public Client(IOptions<BrokerOptions> brokerOptions, ConnectionFactory factory, IConnection connection, IModel channel) : base(brokerOptions, factory, connection, channel)
        {
            //Init();
        }

        public async Task<TResponseEvent> CallAsync<TRequestEvent, TResponseEvent>(TRequestEvent request, IComponentContext context, QueueOptions options) where TRequestEvent : IntegrationEvent
                                                                                                                                                           where TResponseEvent : IntegrationEvent
        {
            MessageSerializer serializaer = new MessageSerializer();

            byte[] byteRequest = serializaer.Serialize(request)[0];
            byte[] byteResponse = await CallAsync(byteRequest, options);

            return serializaer.Deserialize<TResponseEvent>(byteResponse)[0]; ;
        }

        private async Task<byte[]> CallAsync(byte[] message, QueueOptions options)
        {
            byte[] response = null;

            if (options == null)
                throw new ArgumentNullException("Queue configuration parameter cannot be null.");

            byte[] result = new byte[0];

            string replyQueue = string.Empty;
            BlockingCollection<byte[]> respQueue = new BlockingCollection<byte[]>();

            string correlationId = Guid.NewGuid().ToString();

            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);
            IBasicProperties props = Channel.CreateBasicProperties();

            props.CorrelationId = correlationId;
            replyQueue = props.ReplyTo = Channel.QueueDeclare().QueueName;

            consumer.Received += (model, args) =>
            {
                if (args.BasicProperties.CorrelationId == correlationId)
                    respQueue.Add(args.Body.ToArray());
            };

            await Task.Run(() =>
            {
                Channel.BasicPublish("", options.Queue, props, message);
                Channel.BasicConsume(replyQueue, false, consumer);
            });

            response = respQueue.Take();

            return response;
        }
    }
}
