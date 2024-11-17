namespace Defender.Common.Helpers;

public static class Guard
{
    public static void NotNull<T>(T value, string parameterName)
    {
        if (value is null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}
