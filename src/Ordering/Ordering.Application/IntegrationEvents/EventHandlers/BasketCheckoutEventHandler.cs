using EventBus;

using Ordering.Application.IntegrationEvents.Events;
using Ordering.Application.Repositories;
using Ordering.Domain.Models;

using System.Threading.Tasks;

namespace Ordering.Application.IntegrationEvents.EventHandlers
{
    public class BasketCheckoutEventHandler : IIntegrationEventHandler<BasketCheckoutEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public BasketCheckoutEventHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(BasketCheckoutEvent @event)
        {
            await _orderRepository.Save(new Order
            {
                Username = @event.Username,
                TotalPrice = @event.TotalPrice,
                BillingAddress = @event.BillingAddress,
                Payment = @event.Payment
            });
        }
    }
}
