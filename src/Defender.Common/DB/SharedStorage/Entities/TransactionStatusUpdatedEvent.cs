using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Defender.Common.DB.SharedStorage.Enums;
using Defender.Mongo.MessageBroker.Models.TopicMessage;

namespace Defender.Common.DB.SharedStorage.Entities;

public record TransactionStatusUpdatedEvent : BaseTopicMessage
{
    public string? TransactionId { get; set; }
    [BsonRepresentation(BsonType.String)]
    public TransactionStatus TransactionStatus { get; set; }
    [BsonRepresentation(BsonType.String)]
    public TransactionType TransactionType { get; set; }
    [BsonRepresentation(BsonType.String)]
    public TransactionPurpose TransactionPurpose { get; set; }
}


