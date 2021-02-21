using Catalog.Infrastructure.Data.Entities;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class MongoContext
    {
        public MongoContext(IOptions<MongoContextOptions> options)
        {
            var conf = options.Value;

            var db = new MongoClient(conf.ConnectionString).GetDatabase(conf.DatabaseName);

            Products = db.GetCollection<MongoProduct>(conf.ProductsCollection);
        }

        internal IMongoCollection<MongoProduct> Products { get; }
    }
}
