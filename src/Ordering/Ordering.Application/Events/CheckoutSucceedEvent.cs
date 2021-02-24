using EventBus;

namespace Ordering.Application.Events
{
    public class CheckoutSucceedEvent : IntegrationEvent
    {
        public string Username { get; set; }
    }
}
