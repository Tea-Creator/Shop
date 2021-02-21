using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Infrastructure.Data.Entities
{
    internal class MongoProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("image_file")]
        public string ImageFile { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
