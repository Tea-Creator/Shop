namespace Catalog.Infrastructure.Data
{
    public class MongoContextOptions
    {
        public string ProductsCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
