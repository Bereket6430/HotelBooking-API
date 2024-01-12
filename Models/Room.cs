using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Type { get; set; }

    public int Capacity { get; set; }

    public decimal Price { get; set; }

    public bool Available { get; set; }

    
}
