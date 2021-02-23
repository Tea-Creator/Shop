using Microsoft.EntityFrameworkCore;

using Ordering.Infrastructure.Data.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        internal DbSet<OrderEntity> Orders { get; private set; }
    }
}
