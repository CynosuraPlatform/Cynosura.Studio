using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class Solution
    {
        public string Path { get; }
        public string Namespace { get; }
        public List<Project> Projects { get; }

        public Solution(string path)
        {
            Path = path;
            var solutionFile = Directory.GetFiles(Path, "*.sln").FirstOrDefault();
            if (solutionFile == null)
                throw new Exception("Solution file not found");
            Namespace = Regex.Replace(solutionFile, "^.*\\\\([^\\\\]+?).sln$", "$1");
            Projects = GetProjects(Path);
        }

        private List<Project> GetProjects(string path)
        {
            var dirs = Directory.GetDirectories(path);
            var projects = new List<Project>();
            foreach (var dir in dirs)
            {
                var projectFile = Directory.GetFiles(dir, "*.csproj").FirstOrDefault();
                if (projectFile != null)
                {
                    projects.Add(new Project(this, dir, projectFile));
                }
            }

            return projects;
        }

        private static T DeserializeMetadata<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public List<Entities.Entity> GetEntities()
        {
            var coreProject = Projects.Single(p => p.Namespace.EndsWith(".Core"));
            var files = coreProject.GetFiles("Metadata\\Entities");
            var entities = new List<Entities.Entity>();
            foreach (var file in files)
            {
                using (var reader = new StreamReader(file))
                {
                    var entity = DeserializeMetadata<Entities.Entity>(reader.ReadToEnd());
                    entities.Add(entity);
                }
            }

            return entities;
        }
    }
}
