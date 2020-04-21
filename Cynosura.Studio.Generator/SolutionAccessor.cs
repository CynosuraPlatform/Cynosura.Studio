using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Generator.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cynosura.Studio.Generator
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
            Metadata = GetMetadata();
            var solutionFile = System.IO.Path.Combine(Path, Metadata.SolutionFile ?? $"{Metadata.Name}.sln");
            if (!File.Exists(solutionFile))
            {
                throw new Exception("Solution file not found");
            }
            var info = new FileInfo(solutionFile);
            Namespace = Regex.Replace(info.Name, "\\.sln$", "");
            Projects = GetProjects(Path);
        }

        private string GetMetadataPath()
        {
            var newLocation = System.IO.Path.Combine(Path, ".cynosura.json");
            if (File.Exists(newLocation))
            {
                return newLocation;
            }
            var coreDir = Directory.GetDirectories(Path, "*.Core").FirstOrDefault();
            if (coreDir != null)
            {
                var oldLocation = System.IO.Path.Combine(coreDir, "Metadata", "Solution.json");
                if (File.Exists(oldLocation))
                {
                    return oldLocation;
                }
            }
            throw new FileNotFoundException();
        }

        private SolutionMetadata GetMetadata()
        {
            var metadataPath = GetMetadataPath();
            return DeserializeMetadata<SolutionMetadata>(File.ReadAllText(metadataPath));
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

        private static T DeserializeMetadata<T>(string content, params JsonConverter[] converters) where T : new()
        {
            return content.DeserializeFromJson<T>(converters);
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
                var entity = DeserializeMetadata<Entity>(await ReadFileAsync(file), new EntityTypeHandler(Metadata));
                entities.Add(entity);
            }

            var enums = await GetEnumsAsync();

            foreach (var entity in entities)
            {
                var i = 1;

                if (entity.BaseEntityId != null)
                {
                    entity.BaseEntity = entities.First(e => e.Id == entity.BaseEntityId);
                    i += entity.BaseEntity.AllFields.Count + entity.BaseEntity.AllSystemFields.Count;
                }
                
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

                    field.Number = i++;
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

        public async Task<List<Models.View>> GetViewsAsync()
        {
            var coreProject = GetProject("Core");
            var files = coreProject.GetFiles(System.IO.Path.Combine("Metadata", "Views"));
            var views = new List<Models.View>();
            foreach (var file in files)
            {
                var view = DeserializeMetadata<Models.View>(await ReadFileAsync(file));
                views.Add(view);
            }

            if (views.Count == 0)
            {
                var view = new Models.View()
                {
                    Id = Guid.NewGuid(),
                };
                views.Add(view);
                await CreateViewAsync(view);
            }

            return views;
        }

        public async Task CreateViewAsync(Models.View view)
        {
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Views");
            coreProject.VerifyPathExists("Metadata", "Views");
            var filePath = System.IO.Path.Combine(path, view.Name + MetadataFileExtension);
            await WriteFileAsync(filePath, SerializeMetadata(view));
        }

        public async Task UpdateViewAsync(Models.View view)
        {
            var existingView = (await GetViewsAsync()).FirstOrDefault(e => e.Id == view.Id);
            if (existingView == null)
                throw new Exception($"View with Id = {view.Id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Views");
            var filePath = System.IO.Path.Combine(path, view.Name + MetadataFileExtension);
            await WriteFileAsync(filePath, SerializeMetadata(view));

            if (existingView.Name != view.Name)
            {
                filePath = System.IO.Path.Combine(path, existingView.Name + MetadataFileExtension);
                File.Delete(filePath);
            }
        }

        public async Task DeleteViewAsync(Guid id)
        {
            var existingView = (await GetViewsAsync()).FirstOrDefault(e => e.Id == id);
            if (existingView == null)
                throw new Exception($"View with Id = {id} not found");
            var coreProject = GetProject("Core");
            var path = coreProject.GetPath("Metadata", "Views");
            var filePath = System.IO.Path.Combine(path, existingView.Name + MetadataFileExtension);
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
            using (var fileWriter = new StreamWriter(filePath, false, Encoding.UTF8))
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
                    f.TemplatePath = NormalizePath(f.TemplatePath);
                    f.FilePath = NormalizePath(f.FilePath);
                    f.FileName = NormalizePath(f.FileName);
                });
            return templates;
        }

        private string NormalizePath(string path)
        {
            return string.Join(System.IO.Path.DirectorySeparatorChar, path.Split('\\', '/'));
        }

        public string GetTemplatePath(CodeTemplate template)
        {
            var coreProject = GetProject("Core");
            var templatesPath = coreProject.GetPath("Templates");
            return System.IO.Path.Combine(templatesPath, template.TemplatePath);
        }

        public async Task<UpgradeMetadata> GetUpgradeMetadataAsync()
        {
            var upgradePath = System.IO.Path.Combine(Path, "upgrade.json");
            if (!File.Exists(upgradePath))
            {
                return new UpgradeMetadata()
                {
                    Version = 0,
                    Upgrades = new List<UpgradeItem>(),
                };
            }
            var upgradeMetadata = DeserializeMetadata<UpgradeMetadata>(await ReadFileAsync(upgradePath));
            foreach (var upgrade in upgradeMetadata.Upgrades)
            {
                if (upgrade.Renames == null)
                    continue;
                foreach (var upgradeRename in upgrade.Renames)
                {
                    upgradeRename.Left = NormalizePath(upgradeRename.Left);
                    upgradeRename.Right = NormalizePath(upgradeRename.Right);
                }
            }
            return upgradeMetadata;
        }
    }
}
