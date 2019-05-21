using System.IO;
using System.Text.RegularExpressions;

namespace Cynosura.Studio.Generator
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
            var info = new FileInfo(projectFile);
            Namespace = Regex.Replace(info.Name, "\\.csproj$", "");
        }

        public string[] GetFiles(string path)
        {
            var absolutePath = System.IO.Path.Combine(Path, path);
            if (!Directory.Exists(absolutePath))
            {
                return new string[] { };
            }

            return Directory.GetFiles(absolutePath);
        }

        public string GetPath(params string[] pathItems)
        {
            var path = System.IO.Path.Combine(pathItems);
            var absolutePath = System.IO.Path.Combine(Path, path);
            return absolutePath;
        }

        public void VerifyPathExists(params string[] pathItems)
        {
            var path = System.IO.Path.Combine(pathItems);
            var absolutePath = System.IO.Path.Combine(Path, path);
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
            }
        }
    }
}