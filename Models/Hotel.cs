using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Hotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public decimal Price { get; set; }

    
}
