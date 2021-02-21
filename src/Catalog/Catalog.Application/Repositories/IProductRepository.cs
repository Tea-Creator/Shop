using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Catalog.Domain.Models;

namespace Catalog.Application.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> Get(CancellationToken cancellationToken = default);

        Task<Product> Get([NotNull] string id, CancellationToken cancellationToken = default);

        Task<List<Product>> GetByCategory([NotNull] string category, CancellationToken cancellationToken = default);

        Task Create([NotNull] Product product);

        Task<bool> Update([NotNull] Product product);

        Task<bool> Delete([NotNull] string productId);
    }
}
