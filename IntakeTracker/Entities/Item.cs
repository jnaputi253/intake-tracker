using JetBrains.Annotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntakeTracker.Entities
{
    public class Item
    {
        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonElement("category")]
        [BsonRepresentation(BsonType.String)]
        public string Category { get; set; }

        [BsonElement("calories")]
        [BsonRepresentation(BsonType.Int32)]
        public int Calories { get; set; }
    }
}
