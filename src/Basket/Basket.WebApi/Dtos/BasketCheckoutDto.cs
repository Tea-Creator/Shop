using Basket.Domain.Models;

namespace Basket.WebApi.Dtos
{
    public class BasketCheckoutDto
    {
        public string Username { get; set; }
        public Payment Payment { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
