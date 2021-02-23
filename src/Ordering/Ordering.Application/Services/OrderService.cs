using Ordering.Application.DTOs;
using Ordering.Application.Mapping;
using Ordering.Application.Repositories;
using Ordering.Domain.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDto>> GetByUsername(string username, CancellationToken cancellationToken = default)
        {
            var orders = await _repository.GetByUsername(username, cancellationToken);
            return orders.Select(Mapper.Map).ToList();
        }

        public Task Save(Order order)
        {
            return _repository.Save(order);
        }
    }
}
