using Standard.Framework.Seedworks.Concrete.Events;
using System.Threading.Tasks;

namespace Standard.Framework.Seedworks.Abstraction.Events
{
    public interface IIntegrationEventHandler<TResponseEvent> where TResponseEvent : IntegrationEvent
    {
        Task<TResponseEvent> Handle(TResponseEvent @event);
    }

    public interface IIntegrationEventHandler<TRequestEvent, TResponseEvent> where TRequestEvent : IntegrationEvent
                                                                             where TResponseEvent : IntegrationEvent
    {
        Task<TResponseEvent> Handle(TRequestEvent @event);
    }
}
