using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnviroNotify.Dashboard.Database.Entities;

public class EnvironmentData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]   
    public int Id { get; set; }
    public double Humidity { get; set; }
    public double Temperature { get; set; }
    public DateTime DateTime { get; set; }
}