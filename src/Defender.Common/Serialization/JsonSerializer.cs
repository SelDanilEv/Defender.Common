using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;

namespace Defender.Common.Serialization;

public class JsonSerializer<T> : ISerializer<T>, IDeserializer<T>
{
    private readonly ILogger<JsonSerializer<T>> _logger;

    public JsonSerializer(ILogger<JsonSerializer<T>> logger)
    {
        _logger = logger;
    }

    public byte[] Serialize(T data, SerializationContext context)
    {
        try
        {
            var jsonData = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(jsonData);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to serialize JSON data: {Data}", data);
            throw;
        }
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        try
        {
            var jsonData = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<T>(jsonData) ?? throw new JsonException("Deserialized value is null");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize JSON data: {Data}", Encoding.UTF8.GetString(data.ToArray()));
            return default!;
        }
    }
}