namespace Defender.Common.Interfaces;

public interface IMongoSecretAccessor
{
    public Task<string> GetSecretValueAsync(string secretName);
}
