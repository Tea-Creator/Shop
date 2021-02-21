using Catalog.Domain.Models;
using Catalog.Infrastructure.Data.Entities;

namespace Catalog.Infrastructure.Data.Mapping
{
    internal static class Mapper
    {
        public static Product Map(MongoProduct product)
        {
            return new Product
            {
                Id = product.Id,
                Category = product.Category,
                Description = product.Description,
                ImageFile = product.ImageFile,
                Name = product.ImageFile,
                Price = product.Price,
                Summary = product.Summary
            };
        }

        public static MongoProduct Map(Product product)
        {
            return new MongoProduct
            {
                Id = product.Id,
                Category = product.Category,
                Description = product.Description,
                ImageFile = product.ImageFile,
                Name = product.ImageFile,
                Price = product.Price,
                Summary = product.Summary
            };
        }
    }
}
