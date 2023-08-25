using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.Entities;

public interface IBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    Guid Id { get; set; }
}
