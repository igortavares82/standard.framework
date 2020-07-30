using Autofac;
using Microsoft.Extensions.Configuration;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standard.Framework.Seedworks.Concrete.Events;
using System;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Abstraction.RemoteProcedureCall
{
    public interface IServer
    {
        /// <summary>
        /// Recebe chamadas do cliente e retorna uma resposta, sem receber parâmetro.
        /// </summary>
        /// <typeparam name="TRequest">Tipo da mensagem de requisição.</typeparam>
        /// <typeparam name="TResponse">Tipo da mensagem de resposta.</typeparam>
        /// <typeparam name="TIntegrationEventHandler">Tipo de tratamento do evento.</typeparam>
        /// <param name="context">Contexto raiz do injetor dse dependência.</param>
        /// <param name="configureScope">Configuração de escopo de objetos que deverão ser injetados.</param>
        /// <param name="options"~>Opções de fila.</param>
        /// <returns>Mensagem de retorno.</returns>
        Task CallHandlerAsync<TRequest, TResponse, TIntegrationEventHandler>(IComponentContext context,
                                                                             Action<ContainerBuilder, IConfiguration> configureScope,
                                                                             QueueOptions options) where TRequest : IntegrationEvent
                                                                                                   where TResponse : IntegrationEvent;

        /// <summary>
        /// Recebe chamadas do cliente e retorna uma resposta, sem receber parâmetro.
        /// </summary>
        /// <typeparam name="TResponse">Tipo da mensagem de resposta.</typeparam>
        /// <param name="context">Contexto raiz do injetor dse dependência.</param>
        /// <param name="configureScope">Configuração de escopo de objetos que deverão ser injetados.</param>
        /// <param name="options">Opções de fila.</param>
        /// <returns>Mensagem de retorno.</returns>
        Task CallHandlerAsync<TResponse>(IComponentContext context,
                                         Action<ContainerBuilder, IConfiguration> configureScope,
                                         QueueOptions options) where TResponse : IntegrationEvent;
    }
}
