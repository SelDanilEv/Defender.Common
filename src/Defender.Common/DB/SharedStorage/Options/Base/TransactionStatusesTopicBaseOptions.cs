using Defender.Common.DB.SharedStorage.Entities;
using Defender.Common.Enums;
using Defender.Mongo.MessageBroker.Configuration;

namespace Defender.Common.DB.SharedStorage.Options.Base;

public abstract record TransactionStatusesTopicBaseOptions
    : MessageBrokerOptions<TransactionStatusUpdatedEvent>
{
    protected TransactionStatusesTopicBaseOptions(AppEnvironment envPrefix)
    {
        MongoDbDatabaseName = $"{envPrefix}_shared";

        Name = "transaction-statuses";
        Type = "TransactionStatus";

        MaxDocuments = 5000;
        MaxByteSize = int.MaxValue;
    }
}
