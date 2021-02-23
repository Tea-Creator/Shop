using EventBus;

using Ordering.Domain.Models;

namespace Ordering.Application.IntegrationEvents.Events
{
    public class BasketCheckoutEvent : IntegrationEvent
    {
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public Payment Payment { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
