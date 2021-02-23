using Ordering.Domain.Models;
using Ordering.Infrastructure.Data.Entities;

namespace Ordering.Infrastructure.Data.Mapping
{
    internal static class Mapper
    {
        public static Order Map(OrderEntity order)
        {
            return new Order
            {
                Id = order.Id,

                TotalPrice = order.TotalPrice,
                Username = order.Username,

                BillingAddress = new BillingAddress
                {
                    AddressLine = order.AddressLine,
                    Country = order.Country,
                    EmailAddress = order.EmailAddress,
                    FirstName = order.FirstName,
                    LastName = order.LastName,
                    State = order.State,
                    ZipCode = order.ZipCode
                },

                Payment = new Payment
                {
                    CardName = order.CardName,
                    PaymentMethod = order.PaymentMethod,
                    CardNumber = order.CardNumber,
                    CVV = order.CVV,
                    Expiration = order.Expiration
                }
            };
        }

        public static OrderEntity Map(Order order)
        {
            return new OrderEntity
            {
                Id = order.Id,

                TotalPrice = order.TotalPrice,
                Username = order.Username,

                FirstName = order.BillingAddress.FirstName,
                LastName = order.BillingAddress.LastName,
                EmailAddress = order.BillingAddress.EmailAddress,
                AddressLine = order.BillingAddress.AddressLine,
                Country = order.BillingAddress.Country,
                State = order.BillingAddress.State,
                ZipCode = order.BillingAddress.ZipCode,

                CardName = order.Payment.CardName,
                CardNumber = order.Payment.CardNumber,
                Expiration = order.Payment.Expiration,
                CVV = order.Payment.CVV,
                PaymentMethod = (int)order.Payment.PaymentMethod
            };
        }
    }
}
