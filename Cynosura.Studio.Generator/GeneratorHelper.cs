using System.Text.RegularExpressions;

namespace Cynosura.Studio.Generator
{
    public static class GeneratorHelper
    {
        public static string ToLowerCamelCase(this string str)
        {
            if (str == null)
                return null;
            str = Regex.Replace(str, "^.", match => match.Value.ToLower());
            return str;
        }

        public static string ToKebabCase(this string str)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : Regex.Replace(
                        str,
                        "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                        "-$1",
                        RegexOptions.Compiled)
                    .Trim()
                    .ToLower();
        }
    }
}
