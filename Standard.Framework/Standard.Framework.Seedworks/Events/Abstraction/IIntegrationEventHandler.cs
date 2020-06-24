using Standard.Framework.Seedworks.Events.Concrete;
using System.Threading.Tasks;

namespace Standard.Framework.Seedworks.Events.Abstraction
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
