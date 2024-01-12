using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Reservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string HotelId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string RoomId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public decimal TotalCost { get; set; }

    public string Status { get; set; }

    
}
