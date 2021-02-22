using Basket.Application.Services;
using Basket.Domain.Models;
using Basket.WebApi.Dtos;

using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Threading.Tasks;

namespace Basket.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> Get(string username)
        {
            var basket = await _basketService.Get(username);
            return Ok(basket ?? new BasketCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> Update([FromBody] BasketCart cart)
        {
            return Ok(await _basketService.Update(cart));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string username)
        {
            return Ok(await _basketService.Delete(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutDto checkoutDto)
        {
            await _basketService.Checkout(
                checkoutDto.Username, 
                checkoutDto.Payment, 
                checkoutDto.BillingAddress);

            return Accepted();
        }
    }
}
