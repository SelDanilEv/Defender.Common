using Defender.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Defender.Common.Entities.Secrets;

public record MongoSecret : IBaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string? SecretName { get; set; }
    public string? Value { get; set; }

    public static MongoSecret FromSecret(Secret secret, string value)
    {
        return new MongoSecret { SecretName = secret.ToString(), Value = value };
    }

    public static MongoSecret FromSecretName(string secretName, string value)
    {
        return new MongoSecret { SecretName = secretName, Value = value };
    }
}
