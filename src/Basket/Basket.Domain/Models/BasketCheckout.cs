0namespace Basket.Domain.Models
{
    public class BasketCheckout
    {
        public string Username { get; set; }
        public BasketCart Cart { get; set; }
        public Payment Payment { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
