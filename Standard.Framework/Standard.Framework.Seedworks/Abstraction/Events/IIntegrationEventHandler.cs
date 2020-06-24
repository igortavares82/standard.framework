using Standard.Framework.Seedworks.Concrete.Events;
using System.Threading.Tasks;

namespace Standard.Framework.Seedworks.Abstraction.Events
{
    public interface IIntegrationEventHandler<TEventResponse> where TEventResponse : IntegrationEvent
    {
        Task<TEventResponse> Handle(TEventResponse @event);
    }

    public interface IIntegrationEventHandler<TEventRequest, TEventResponse> where TEventRequest : IntegrationEvent
                                                                             where TEventResponse : IntegrationEvent
    {
        Task<TEventResponse> Handle(TEventRequest @event);
    }
}
