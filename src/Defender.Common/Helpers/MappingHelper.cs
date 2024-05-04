namespace Defender.Common.Helpers;

public static class MappingHelper
{
    public static T2 MapEnum<T1, T2>(T1 enumV) where T1 : Enum where T2 : struct, Enum
    {
        return Enum.Parse<T2>(enumV.ToString());
    }
}
