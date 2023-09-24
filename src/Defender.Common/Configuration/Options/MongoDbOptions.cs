namespace Defender.Common.Configuration.Options;

public class MongoDbOptions
{
    public string AppName { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}
