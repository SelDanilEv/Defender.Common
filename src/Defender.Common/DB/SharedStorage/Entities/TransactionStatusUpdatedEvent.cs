using Defender.Common.DB.SharedStorage.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.DB.SharedStorage.Entities;

public record TransactionStatusUpdatedEvent
{
    public string? TransactionId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public TransactionStatus TransactionStatus { get; set; }
    [BsonRepresentation(BsonType.String)]
    public TransactionType TransactionType { get; set; }
    [BsonRepresentation(BsonType.String)]
    public TransactionPurpose TransactionPurpose { get; set; }
}


