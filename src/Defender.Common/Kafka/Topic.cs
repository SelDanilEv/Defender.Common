namespace Defender.Common.Kafka;

public enum Topic
{
    CorrelatedResponses,
    TransactionStatusUpdates,
}

public static class TopicExtensions
{
    private static readonly Dictionary<Topic, string> TopicToStringMap =
        new()
        {
            { Topic.CorrelatedResponses, "correlated-responses-topic" },
            { Topic.TransactionStatusUpdates, "transaction-status-updates-topic" },
        };

    public static string GetName(this Topic topic)
    {
        if (TopicToStringMap.TryGetValue(topic, out var name))
        {
            return name;
        }
        throw new ArgumentException($"Unknown topic: {topic}");
    }

    public static Topic ToTopic(this string topic)
    {
        var stringToTopicMap = TopicToStringMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        if (stringToTopicMap.TryGetValue(topic, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Unknown topic: {topic}");
    }
}
