using EventBus;

namespace Basket.Application.IntegrationEvents.Events
{
    public class CheckoutSucceedEvent : IntegrationEvent
    {
        public string Username { get; set; }
    }
}
