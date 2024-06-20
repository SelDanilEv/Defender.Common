using Defender.Common.DB.SharedStorage.Options.Base;
using Defender.Common.Enums;
using Defender.Common.Helpers;

namespace Defender.Common.DB.SharedStorage.Options;

public record TransactionStatusesTopicConsumerOptions
    : TransactionStatusesTopicBaseOptions
{
    public TransactionStatusesTopicConsumerOptions(AppEnvironment envPrefix)
        : base(envPrefix)
    {
        MongoDbConnectionString = SecretsHelper.GetSecretSync(
            Secret.SharedROConnectionString, true);
    }
}
