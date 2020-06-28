using Autofac;
using Standand.Framework.MessageBroker.Concrete.Options;
using Standard.Framework.Seedworks.Concrete.Events;
using System.Threading.Tasks;

namespace Standand.Framework.MessageBroker.Abstraction.Queue
{
    /// <summary>
    /// Componente responsável por consumir eventos (mensagens) do barramento. Comunicação em modo assíncrono.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publicação de mensagem unitária no barramento.
        /// </summary>
        /// <typeparam name="TRequestEvent"></typeparam>
        /// <param name="request">Mensagem a ser enviada, a qual será serializada em json antes do envio.</param>
        /// <param name="context">Contexto do injetor de dependência.</param>
        /// <param name="options">Possibilidade de sobrescrever as configurações de instância.</param>
        Task PublishAsync<TRequestEvent>(TRequestEvent request, IComponentContext context, QueueOptions options = null) where TRequestEvent : IntegrationEvent;
    }
}
