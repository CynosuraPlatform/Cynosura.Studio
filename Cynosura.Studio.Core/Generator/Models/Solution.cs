using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
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

        private const string MetadataFileExtension = ".json";

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

        private static string SerializeMetadata<T>(T data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        private Project GetProject(string name)
        {
            return Projects.Single(p => p.Namespace.EndsWith("." + name));
        }

        public List<Entity> GetEntities()
        {
            var coreProject = GetProject("Core");
            var files = coreProject.GetFiles("Metadata\\Entities");
            var entities = new List<Entity>();
            foreach (var file in files)
            {
                using (var reader = new StreamReader(file))
                {
                    var entity = DeserializeMetadata<Entity>(reader.ReadToEnd());
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void CreateEntity(Entity entity)
        {
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata\\Entities");
            coreProject.VerifyPathExists("Metadata\\Entities");
            var filePath = System.IO.Path.Combine(path, entity.Name + MetadataFileExtension);
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(SerializeMetadata(entity));
            }
        }

        public void UpdateEntity(Entity entity)
        {
            var existingEntity = GetEntities().FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity == null)
                throw new Exception($"Entity with Id = {entity.Id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata\\Entities");
            var filePath = System.IO.Path.Combine(path, entity.Name + MetadataFileExtension);
            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(SerializeMetadata(entity));
            }

            if (existingEntity.Name != entity.Name)
            {
                filePath = System.IO.Path.Combine(path, existingEntity.Name + MetadataFileExtension);
                File.Delete(filePath);
            }
        }

        public void DeleteEntity(Guid id)
        {
            var existingEntity = GetEntities().FirstOrDefault(e => e.Id == id);
            if (existingEntity == null)
                throw new Exception($"Entity with Id = {id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata\\Entities");
            var filePath = System.IO.Path.Combine(path, existingEntity.Name + MetadataFileExtension);
            File.Delete(filePath);
        }
    }
}
