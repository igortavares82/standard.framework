using Standand.Framework.MessageBroker.Concrete.Options;
using Standard.Framework.Seedworks.Abstraction.Events;
using Standard.Framework.Seedworks.Concrete.Events;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Abstraction
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent @event, QueueOptions options);

        Task SubscribeAsync<TIntegrationEvent, TIntegrationEventHandler>(QueueOptions options) where TIntegrationEvent : IntegrationEvent
                                                                                               where TIntegrationEventHandler : IIntegrationEventHandler<TIntegrationEvent>;

        Task SubscribeAsync<TRequestEvent, TResponseEvent, TIntegrationEventHandler>(QueueOptions options) where TRequestEvent : IntegrationEvent
                                                                                                           where TResponseEvent : IntegrationEvent
                                                                                                           where TIntegrationEventHandler : IIntegrationEventHandler<TRequestEvent, TResponseEvent>;

        Task<TResponseEvent> CallAsync<TRequestEvent, TResponseEvent>(TRequestEvent request, QueueOptions options) where TRequestEvent : IntegrationEvent
                                                                                                                   where TResponseEvent : IntegrationEvent;
    }
}
