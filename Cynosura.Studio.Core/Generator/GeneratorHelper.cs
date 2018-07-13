using System.Text.RegularExpressions;

namespace Cynosura.Studio.Core.Generator
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
    }
}
