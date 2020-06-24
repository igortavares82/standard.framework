using Autofac;
using Microsoft.Extensions.Configuration;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standard.Framework.Seedworks.Events.Abstraction;
using Standard.Framework.Seedworks.Events.Concrete;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Abstraction.Queue
{
    /// <summary>
    /// Componente responsável por consumir eventos (mensagens) do barramento. Comunicação em modo assíncrono.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Assinatura de um determinado evento.
        /// </summary>
        /// <typeparam name="TIntegrationEventHandler">Tipo do evento assinado.</typeparam>
        /// <typeparam name="TRequestEvent">Tipo de tratamento do evento.</typeparam>
        /// <param name="context">Contexto raiz do injetor dse dependência.</param>
        /// <param name="configureScope">Configuração de escopo de objetos que deverão ser injetados.</param>
        /// <param name="options">Opções de fila.</param>
        Task SubscribeAsync<TRequestEvent, TIntegrationEventHandler>(IComponentContext context,
                                                                         Action<ContainerBuilder, IConfiguration> configureScope,
                                                                         QueueOptions options = null) where TRequestEvent : IntegrationEvent
                                                                                                      where TIntegrationEventHandler : IIntegrationEventHandler<TRequestEvent>;

    }
}
