using Autofac;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Abstraction.RemoteProcedureCall
{
    /// <summary>
    /// Componente responsável por realizar chamadas síncronas a um determinado servidor.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Executar uma chamada ao servidor em duas vias, com retorno conforme tipo esperado.
        /// </summary>
        /// <typeparam name="TRequest">Tipo da mensagem a ser enviada.</typeparam>
        /// <typeparam name="TResponse">Tipo da mensagem a ser retornada.</typeparam>
        /// <param name="request">Mensagem de retorno.</param>
        /// <param name="context">Contexto do injetor de dependência.</param>
        /// <param name="options">Opções de chamada.</param>
        /// <returns>Retorno da chamada</returns>
        Task<TResponseEvent> CallAsync<TRequestEvent, TResponseEvent>(TRequestEvent request, IComponentContext context, QueueOptions options) where TRequestEvent : IntegrationEvent
                                                                                                                                              where TResponseEvent : IntegrationEvent;
    }
}
