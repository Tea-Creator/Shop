namespace Ordering.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public Payment Payment { get; set; }
    }
}
