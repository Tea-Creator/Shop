using System.Collections.Generic;
using System.Linq;

namespace Basket.Domain.Models
{
    public class BasketCart
    {
        public BasketCart() : this(null)
        {
        }

        public BasketCart(string username)
        {
            Username = username;

            Items = new List<BasketCartItem>();
        }

        public string Username { get; set; }
        public List<BasketCartItem> Items { get; set; }

        public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
    }
}
