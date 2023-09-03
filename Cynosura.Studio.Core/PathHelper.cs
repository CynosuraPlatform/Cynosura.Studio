using System;
using System.IO;
using System.Reflection;

namespace Cynosura.Studio.Core
{
    public static class PathHelper
    {
        private static string? AssemblyDirectory
        {
            get
            {
                var location = Assembly.GetEntryAssembly()?.Location;
                if (location == null)
                {
                    return null;
                }
                var uri = new UriBuilder(location);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string GetAbsolutePath(string path1)
        {
            if (!Path.IsPathRooted(path1) && AssemblyDirectory != null)
            {
                return Path.Combine(AssemblyDirectory, path1);
            }
            return path1;
        }
    }
}
