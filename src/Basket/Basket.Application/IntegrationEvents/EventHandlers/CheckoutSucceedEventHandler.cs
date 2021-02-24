using Basket.Application.IntegrationEvents.Events;
using Basket.Application.Services;

using EventBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.IntegrationEvents.EventHandlers
{
    public class CheckoutSucceedEventHandler : IIntegrationEventHandler<CheckoutSucceedEvent>
    {
        private readonly BasketService _basketService;

        public CheckoutSucceedEventHandler(BasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task Handle(CheckoutSucceedEvent @event)
        {
            await _basketService.Delete(@event.Username);
        }
    }
}
