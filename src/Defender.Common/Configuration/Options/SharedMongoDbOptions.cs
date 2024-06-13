using Defender.Common.Consts;

namespace Defender.Common.Configuration.Options;

public record SharedMongoDbOptions : MongoDbOptions
{
    public SharedMongoDbOptions()
    {
    }

    public SharedMongoDbOptions(MongoDbOptions options)
        : base(ConstValues.SharedDatabaseName, options)
    {
    }
}
