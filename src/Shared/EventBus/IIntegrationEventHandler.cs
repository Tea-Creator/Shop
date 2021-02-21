using System.Threading.Tasks;

namespace EventBus
{
    public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        Task Handle(TEvent @event);
    }
}
