using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Catalog.Application.Exceptions;
using Catalog.Application.Repositories;
using Catalog.Domain.Models;
using Catalog.Infrastructure.Data.Mapping;

using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Repositories
{
    public class MongoProductRepository : IProductRepository
    {
        private readonly MongoContext _db;

        public MongoProductRepository(MongoContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> Get(CancellationToken cancellationToken = default)
        {
            var products = await _db.Products
                .Find(_ => true)
                .ToListAsync(cancellationToken);

            return products.Select(Mapper.Map).ToList();
        }

        public async Task<Product> Get([NotNull] string id, CancellationToken cancellationToken = default)
        {
            var product = await _db.Products.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);

            if (product is null)
            {
                throw new EntityNotFoundException(typeof(Product), searchParams: id);
            }

            return Mapper.Map(product);
        }

        public async Task<List<Product>> GetByCategory([NotNull] string category, CancellationToken cancellationToken = default)
        {
            var products = await _db.Products
                .Find(p => p.Category == category)
                .ToListAsync(cancellationToken);

            return products.Select(Mapper.Map).ToList();
        }

        public async Task Create([NotNull] Product product)
        {
            var insertValue = Mapper.Map(product);

            await _db.Products.InsertOneAsync(insertValue);

            product.Id = insertValue.Id;
        }

        public async Task<bool> Update([NotNull] Product product)
        {
            var result = await _db.Products
                .ReplaceOneAsync(p => p.Id == product.Id, Mapper.Map(product));

            return result.IsAcknowledged && result.MatchedCount != 0;
        }

        public async Task<bool> Delete([NotNull] string productId)
        {
            var result = await _db.Products
                .DeleteOneAsync(p => p.Id == productId);

            return result.IsAcknowledged && result.DeletedCount != 0;
        }
    }
}
