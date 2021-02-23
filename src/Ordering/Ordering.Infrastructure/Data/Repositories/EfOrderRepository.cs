using Microsoft.EntityFrameworkCore;

using Ordering.Application.Repositories;
using Ordering.Domain.Models;
using Ordering.Infrastructure.Data.Mapping;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Repositories
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly OrderContext _db;

        public EfOrderRepository(OrderContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> GetByUsername(string username, CancellationToken cancellationToken = default)
        {
            var orders = await _db.Orders
                .Where(order => order.Username == username)
                .ToListAsync(cancellationToken);

            return orders.Select(Mapper.Map).ToList();
        }

        public async Task Save(Order order)
        {
            _db.Orders.Add(Mapper.Map(order));

            await _db.SaveChangesAsync();
        }
    }
}
