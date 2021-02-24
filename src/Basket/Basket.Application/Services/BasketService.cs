using Basket.Application.Events;
using Basket.Application.Repositories;
using Basket.Domain.Models;

using EventBus;

using System.Threading.Tasks;

namespace Basket.Application.Services
{
    public class BasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IEventBus _eventBus;

        public BasketService(
            IBasketRepository basketRepository,
            IEventBus eventBus)
        {
            _basketRepository = basketRepository;
            _eventBus = eventBus;
        }

        public async Task<BasketCart> Get(string username)
        {
            return await _basketRepository.Get(username);
        }

        public async Task<BasketCart> Update(BasketCart cart)
        {
            return await _basketRepository.Update(cart);
        }

        public async Task<bool> Delete(string username)
        {
            return await _basketRepository.Delete(username);
        }

        public async Task Checkout(string username, Payment payment, BillingAddress billingAddress)
        {
            var cart = await Get(username);

            if (cart is null)
            {
                return;
            }

            var checkoutEvent = new BasketCheckoutEvent
            {
                Username = username,
                Payment = payment,
                BillingAddress = billingAddress,
                TotalPrice = cart.TotalPrice
            };

            _eventBus.Publish(checkoutEvent);
        }
    }
}
