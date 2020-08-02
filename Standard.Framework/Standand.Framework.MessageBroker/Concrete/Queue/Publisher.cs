using Autofac;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Standand.Framework.MessageBroker.Abstraction;
using Standand.Framework.MessageBroker.Abstraction.Queue;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standand.Framework.MessageBroker.Concrete.Serializers;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Concrete.Queue
{
    public class Publisher : Broker, IPublisher
    {
        public Publisher(IOptions<BrokerOptions> brokerOptions, ConnectionFactory factory, IConnection connection, IModel channel) : base(brokerOptions, factory, connection, channel)
        {
            //Init();
        }

        public async Task PublishAsync<TRequestEvetn>(TRequestEvetn request, IComponentContext context, QueueOptions options = null) where TRequestEvetn : IntegrationEvent
        {
            MessageSerializer serializaer = new MessageSerializer();
            await PublishAsync(serializaer.Serialize(request), options);
        }

        public async Task PublishAsync(List<byte[]> messages, QueueOptions options = null)
        {
            if (options == null && QueueOptions == null)
                throw new ArgumentNullException("Queue options cannot be null.");

            QueueOptions qo = options ?? QueueOptions;
            Channel.QueueDeclare(qo.Queue, qo.Durable, qo.Exclusive, qo.AutoDelete, null);
            await Task.Run(() => messages.ForEach(it => Channel.BasicPublish("", qo.Queue, true, null, it)));
        }
    }
}
