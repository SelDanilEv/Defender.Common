namespace Defender.Common.Helpers;

public static class MappingHelper
{
    public static T2 MapEnum<T1, T2>(T1 enumV, T2 defaultV = default) where T1 : Enum where T2 : struct, Enum
    {
        var isDefined = Enum.IsDefined(typeof(T2), enumV.ToString());

        return isDefined ? Enum.Parse<T2>(enumV.ToString()) : defaultV;
    }
}
