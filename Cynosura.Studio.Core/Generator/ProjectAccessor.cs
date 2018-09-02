using System.IO;
using System.Text.RegularExpressions;

namespace Cynosura.Studio.Core.Generator
{
    public class ProjectAccessor
    {
        public string Path { get; }
        public string Namespace { get; }
        public SolutionAccessor Solution { get; }

        internal ProjectAccessor(SolutionAccessor solution, string path, string projectFile)
        {
            Solution = solution;
            Path = path;
            Namespace = Regex.Replace(projectFile, "^.*\\\\([^\\\\]+?).csproj", "$1");
        }

        public string[] GetFiles(string path)
        {
            var absolutePath = System.IO.Path.Combine(Path, path);
            if (!Directory.Exists(absolutePath))
            {
                return new string[] {};
            }
            return Directory.GetFiles(absolutePath);
        }

        public string GetPath(string path)
        {
            var absolutePath = System.IO.Path.Combine(Path, path);
            return absolutePath;
        }

        public void VerifyPathExists(string path)
        {
            var absolutePath = System.IO.Path.Combine(Path, path);
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
        }
    }
}
