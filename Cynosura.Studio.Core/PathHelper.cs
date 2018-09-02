using System;
using System.IO;
using System.Reflection;

namespace Cynosura.Studio.Core
{
    public static class PathHelper
    {
        private static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetEntryAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string GetAbsolutePath(string path1)
        {
            if (!Path.IsPathRooted(path1))
            {
                return Path.Combine(AssemblyDirectory, path1);
            }
            return path1;
        }
    }
}
