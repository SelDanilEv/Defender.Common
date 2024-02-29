namespace Defender.Common.Configuration.Options;

public record MongoDbOptions
{
    public MongoDbOptions()
    {
    }

    public MongoDbOptions(string appName, MongoDbOptions options)
    {
        AppName = appName;
        Environment = options.Environment;
        ConnectionString = options.ConnectionString;
    }

    public string AppName { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;

    public string GetDatabaseName() => $"{Environment}_{AppName}";
}
