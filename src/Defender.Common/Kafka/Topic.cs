namespace Defender.Common.Kafka;

public enum Topic
{
    CorrelatedResponses
}

public static class TopicExtensions
{
    private static class Topics
    {
        public const string CorrelatedResponses = "correlated-responses-topic";
    }

    private static readonly Dictionary<Topic, string> _topicToStringMap =
        new()
        {
            { Topic.CorrelatedResponses, Topics.CorrelatedResponses },
        };


    public static string GetName(this Topic topic)
    {
        if (_topicToStringMap.TryGetValue(topic, out var name))
        {
            return name;
        }
        throw new ArgumentException($"Unknown topic: {topic}");
    }

    public static Topic ToTopic(this string topic)
    {
        var stringToTopicMap = _topicToStringMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        if (stringToTopicMap.TryGetValue(topic, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Unknown topic: {topic}");
    }
}
