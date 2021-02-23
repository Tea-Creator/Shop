using Ordering.Domain.Models;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByUsername(string username, CancellationToken cancellationToken = default);

        Task Save(Order order);
    }
}
