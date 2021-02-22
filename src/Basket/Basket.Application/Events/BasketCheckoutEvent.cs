using Basket.Domain.Models;

using EventBus;

namespace Basket.Application.Events
{
    public class BasketCheckoutEvent : IntegrationEvent
    {
        public string Username { get; set; }
        public BasketCart Cart { get; set; }
        public Payment Payment { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
