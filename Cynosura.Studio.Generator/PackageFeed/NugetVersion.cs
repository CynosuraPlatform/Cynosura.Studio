using System.Text.RegularExpressions;

namespace Cynosura.Studio.Generator.PackageFeed
{
    public class NugetVersion
    {
        public NugetVersion(string version)
        {
            var dots = version.Split(".");
            Original = version;
            Major = int.Parse(dots[0]);
            Minor = int.Parse(dots[1]);
            if (dots.Length > 2)
                Maintenance = int.Parse(dots[2]);
            var match = Pattern.Match(version);
            Alpha = match.Groups[1].Value;
        }

        public static Regex Pattern = new Regex("\\d+\\.\\d+(\\.\\d+)?\\-?(.*)");
        
        public static bool IsValid(string version)
        {
            return Pattern.IsMatch(version);
        }
        public string Original { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Maintenance { get; set; }
        public int Build { get; set; }
        
        public string Alpha { get; set; }
        public bool IsPrerelease => !string.IsNullOrEmpty(Alpha);

        public int Version => Major * 10000 + Minor * 100 + Maintenance + (IsPrerelease ? 1 : 0);
    }
}
