using System.Text.RegularExpressions;

namespace Defender.Common.Extension;

public static class BasicTypesExtensions
{
    public static string DigitsOnly(this string str)
    {
        var pattern = @"[^\d]";
        var replacement = "";
        var regex = new Regex(pattern);
        var result = regex.Replace(str, replacement);

        return result;
    }

}