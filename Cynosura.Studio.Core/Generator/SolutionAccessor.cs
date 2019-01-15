using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Generator.Models;
using Newtonsoft.Json;

namespace Cynosura.Studio.Core.Generator
{
    public class SolutionAccessor
    {
        public string Path { get; }
        public SolutionMetadata Metadata { get; }
        public string Namespace { get; }
        public List<ProjectAccessor> Projects { get; }

        private const string MetadataFileExtension = ".json";

        public SolutionAccessor(string path)
        {
            Path = path;
            var solutionFile = Directory.GetFiles(Path, "*.sln").FirstOrDefault();
            if (solutionFile == null)
                throw new Exception("Solution file not found");
            var info = new FileInfo(solutionFile);
            Namespace = Regex.Replace(info.Name, "\\.sln$", "");
            Projects = GetProjects(Path);
            Metadata = GetMetadataAsync().Result; // TODO: remove .Result
        }

        private async Task<SolutionMetadata> GetMetadataAsync()
        {
            var coreProject = GetProject("Core");
            var metadataPath = System.IO.Path.Combine(coreProject.Path, "Metadata/Solution.json");
            return DeserializeMetadata<SolutionMetadata>(await ReadFileAsync(metadataPath));
        }

        private List<ProjectAccessor> GetProjects(string path)
        {
            var dirs = Directory.GetDirectories(path);
            var projects = new List<ProjectAccessor>();
            foreach (var dir in dirs)
            {
                var projectFile = Directory.GetFiles(dir, "*.csproj").FirstOrDefault();
                if (projectFile != null)
                {
                    projects.Add(new ProjectAccessor(this, dir, projectFile));
                }
            }

            return projects;
        }

        private static T DeserializeMetadata<T>(string content) where T : new()
        {
            return content.DeserializeFromJson<T>();
        }

        private static string SerializeMetadata<T>(T data)
        {
            return data.SerializeToJson();
        }

        private ProjectAccessor GetProject(string name)
        {
            return Projects.First(p => p.Namespace.EndsWith("." + name));
        }

        public async Task<List<Entity>> GetEntitiesAsync()
        {
            var coreProject = GetProject("Core");
            var files = coreProject.GetFiles(System.IO.Path.Combine("Metadata", "Entities"));
            var entities = new List<Entity>();
            foreach (var file in files)
            {
                var entity = DeserializeMetadata<Entity>(await ReadFileAsync(file));
                entities.Add(entity);
            }

            var enums = await GetEnumsAsync();

            foreach (var entity in entities)
            {
                foreach (var field in entity.Fields)
                {
                    if (field.EntityId != null)
                    {
                        field.Entity = entities.First(e => e.Id == field.EntityId);
                    }

                    if (field.EnumId != null)
                    {
                        field.Enum = enums.First(e => e.Id == field.EnumId);
                    }
                }
            }

            return entities;
        }

        public async Task CreateEntityAsync(Entity entity)
        {
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Entities");
            coreProject.VerifyPathExists("Metadata", "Entities");
            var filePath = System.IO.Path.Combine(path, entity.Name + MetadataFileExtension);
            await WriteFileAsync(filePath, SerializeMetadata(entity));
        }

        public async Task UpdateEntityAsync(Entity entity)
        {
            var existingEntity = (await GetEntitiesAsync()).FirstOrDefault(e => e.Id == entity.Id);
            if (existingEntity == null)
                throw new Exception($"Entity with Id = {entity.Id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Entities");
            var filePath = System.IO.Path.Combine(path, entity.Name + MetadataFileExtension);
            await WriteFileAsync(filePath, SerializeMetadata(entity));

            if (existingEntity.Name != entity.Name)
            {
                filePath = System.IO.Path.Combine(path, existingEntity.Name + MetadataFileExtension);
                File.Delete(filePath);
            }
        }

        public async Task DeleteEntityAsync(Guid id)
        {
            var existingEntity = (await GetEntitiesAsync()).FirstOrDefault(e => e.Id == id);
            if (existingEntity == null)
                throw new Exception($"Entity with Id = {id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Entities");
            var filePath = System.IO.Path.Combine(path, existingEntity.Name + MetadataFileExtension);
            File.Delete(filePath);
        }

        public async Task<List<Models.Enum>> GetEnumsAsync()
        {
            var coreProject = GetProject("Core");
            var files = coreProject.GetFiles(System.IO.Path.Combine("Metadata", "Enums"));
            var enums = new List<Models.Enum>();
            foreach (var file in files)
            {
                var @enum = DeserializeMetadata<Models.Enum>(await ReadFileAsync(file));
                enums.Add(@enum);
            }

            return enums;
        }

        public async Task CreateEnumAsync(Models.Enum @enum)
        {
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Enums");
            coreProject.VerifyPathExists("Metadata", "Enums");
            var filePath = System.IO.Path.Combine(path, @enum.Name + MetadataFileExtension);
            await WriteFileAsync(filePath, SerializeMetadata(@enum));
        }

        public async Task UpdateEnumAsync(Models.Enum @enum)
        {
            var existingEnum = (await GetEnumsAsync()).FirstOrDefault(e => e.Id == @enum.Id);
            if (existingEnum == null)
                throw new Exception($"Enum with Id = {@enum.Id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Enums");
            var filePath = System.IO.Path.Combine(path, @enum.Name + MetadataFileExtension);
            await WriteFileAsync(filePath, SerializeMetadata(@enum));

            if (existingEnum.Name != @enum.Name)
            {
                filePath = System.IO.Path.Combine(path, existingEnum.Name + MetadataFileExtension);
                File.Delete(filePath);
            }
        }

        public async Task DeleteEnumAsync(Guid id)
        {
            var existingEnum = (await GetEnumsAsync()).FirstOrDefault(e => e.Id == id);
            if (existingEnum == null)
                throw new Exception($"Enum with Id = {id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Enums");
            var filePath = System.IO.Path.Combine(path, existingEnum.Name + MetadataFileExtension);
            File.Delete(filePath);
        }

        private async Task<string> ReadFileAsync(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return await fileReader.ReadToEndAsync();
            }
        }

        private async Task WriteFileAsync(string filePath, string content)
        {
            using (var fileWriter = new StreamWriter(filePath))
            {
                await fileWriter.WriteAsync(content);
            }
        }

        public async Task<IEnumerable<CodeTemplate>> LoadTemplatesAsync()
        {
            var coreProject = GetProject("Core");
            var templatesPath = coreProject.GetPath("Templates");
            var templatesJson = await ReadFileAsync(System.IO.Path.Combine(templatesPath, "Templates.json"));
            var templates = DeserializeMetadata<List<CodeTemplate>>(templatesJson);
            templates.ForEach(f =>
                {
                    f.TemplatePath = string.Join(System.IO.Path.DirectorySeparatorChar,
                        f.TemplatePath.Split('\\', '/'));
                    f.FilePath = string.Join(System.IO.Path.DirectorySeparatorChar,
                        f.FilePath.Split('\\', '/'));
                });
            return templates;
        }

        public string GetTemplatePath(CodeTemplate template)
        {
            var coreProject = GetProject("Core");
            var templatesPath = coreProject.GetPath("Templates");
            return System.IO.Path.Combine(templatesPath, template.TemplatePath);
        }
    }
}