using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnviroNotify.Database.Entities;

public class PersistedClient
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Endpoint { get; set; }

    public string P256DH { get; set; }

    public string Auth { get; set; }
}