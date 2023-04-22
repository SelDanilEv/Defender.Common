using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.Entities;

public interface IBaseModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
}
