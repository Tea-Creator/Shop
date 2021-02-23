using Ordering.Application.DTOs;
using Ordering.Domain.Models;

namespace Ordering.Application.Mapping
{
    public static class Mapper
    {
        public static OrderDto Map(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,

                Username = order.Username,
                TotalPrice = order.TotalPrice,

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
