using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Core.Services;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Generator.Infrastructure;
using Cynosura.Studio.Generator.Merge;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Generator.TemplateEngine;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Generator
{
    public class CodeGenerator
    {
        private readonly ITemplateEngine _templateEngine;
        private readonly IDirectoryMerge _directoryMerge;
        private readonly ILogger<CodeGenerator> _logger;
        public CodeGenerator(ITemplateEngine templateEngine,
            IDirectoryMerge directoryMerge,
            ILogger<CodeGenerator> logger)
        {
            _templateEngine = templateEngine;
            _directoryMerge = directoryMerge;
            _logger = logger;
        }

        private string ProcessTemplate(CodeTemplate template, SolutionAccessor solutionAccessor, object model)
        {
            var templatePath = solutionAccessor.GetTemplatePath(template);
            return _templateEngine.ProcessTemplate(templatePath, model);
        }

        internal string GetTemplateFilePath(CodeTemplate template, SolutionAccessor solution, IGenerationObject generationObject)
        {
            var dir = FindDirectory(solution.Path, template.FilePath);
            var fileName = generationObject.ProcessTemplate(template.FileName);
            var filePath = Path.Combine(dir, fileName);
            var fileDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
            return filePath;
        }

        private async Task CreateFileAsync(CodeTemplate template, object model, SolutionAccessor solution, IGenerationObject generationObject, string overrideSolutionPath = null)
        {
            var filePath = GetTemplateFilePath(template, solution, generationObject);
            var fileSavePath = filePath;
            if (!string.IsNullOrEmpty(overrideSolutionPath))
            {
                fileSavePath = Path.Combine(overrideSolutionPath, Path.GetRelativePath(solution.Path, fileSavePath));
                var dir = Path.GetDirectoryName(fileSavePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
            var content = ProcessTemplate(template, solution, model);

            if (!string.IsNullOrEmpty(template.InsertAfter))
            {
                var fileContent = await ReadFileAsync(filePath);

                if (!fileContent.Contains(content))
                {
                    fileContent = fileContent.Replace(template.InsertAfter + Environment.NewLine,
                        template.InsertAfter + Environment.NewLine + content + Environment.NewLine);

                    await WriteFileAsync(fileSavePath, fileContent);
                }
            }
            else
            {
                await WriteFileAsync(fileSavePath, content);
            }
        }

        

        

        private IEnumerable<Entity> SortByDependency(IEnumerable<Entity> entities)
        {
            var entityList = entities.ToList();
            var sortedList = new List<Entity>();
            while (entityList.Count > 0)
            {
                var okEntities = entityList
                    .Where(e => e.DependentEntities.Count == 0 ||
                                e.DependentEntities.All(de => sortedList.Contains(de)))
                    .ToList();
                if (okEntities.Count == 0)
                    throw new Exception("Cannot sort by dependency");
                foreach (var okEntity in okEntities)
                {
                    entityList.Remove(okEntity);
                    sortedList.Add(okEntity);
                }
            }

            return sortedList;
        }

        internal async Task CopyEntitiesAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
        {
            var fromEntities = await fromSolution.GetEntitiesAsync();
            fromEntities = SortByDependency(fromEntities).ToList();
            var toEntities = await toSolution.GetEntitiesAsync();
            var oldEntitiesToUpgrade = new List<Entity>();
            var newEntitiesToUpgrade = new List<Entity>();
            foreach (var entity in fromEntities)
            {
                var toEntity = toEntities.FirstOrDefault(e => e.Id == entity.Id);
                if (toEntity == null)
                {
                    await toSolution.CreateEntityAsync(entity);
                    var newEntity = (await toSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    await GenerateEntityAsync(toSolution, newEntity);
                    await GenerateEntityViewAsync(toSolution, newEntity);
                }
                else
                {
                    await toSolution.UpdateEntityAsync(entity);
                    var newEntity = (await toSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    oldEntitiesToUpgrade.Add(toEntity);
                    newEntitiesToUpgrade.Add(newEntity);
                }
            }

            if (oldEntitiesToUpgrade.Count > 0)
            {
                var oldGenerateInfos = oldEntitiesToUpgrade
                    .SelectMany(oldEntity => new[] {
                        new EntityModel(oldEntity, toSolution).GetGenerateInfo(),
                        new EntityViewModel(new View(), oldEntity, toSolution).GetGenerateInfo()
                    });
                var newGenerateInfos = newEntitiesToUpgrade
                    .SelectMany(newEntity => new[] {
                        new EntityModel(newEntity, toSolution).GetGenerateInfo(),
                        new EntityViewModel(new View(), newEntity, toSolution).GetGenerateInfo()
                    });
                await UpgradeAsync(toSolution, oldGenerateInfos, newGenerateInfos);
            }
        }

        internal async Task CopyEnumsAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
        {
            var fromEnums = await fromSolution.GetEnumsAsync();
            var toEnums = await toSolution.GetEnumsAsync();
            var oldEnumsToUpgrade = new List<Models.Enum>();
            var newEnumsToUpgrade = new List<Models.Enum>();
            foreach (var @enum in fromEnums)
            {
                var toEnum = toEnums.FirstOrDefault(e => e.Id == @enum.Id);
                if (toEnum == null)
                {
                    await toSolution.CreateEnumAsync(@enum);
                    var newEnum = (await toSolution.GetEnumsAsync())
                        .FirstOrDefault(e => e.Id == @enum.Id);
                    await GenerateEnumAsync(toSolution, newEnum);
                    await GenerateEnumViewAsync(toSolution, newEnum);
                }
                else
                {
                    await toSolution.UpdateEnumAsync(@enum);
                    var newEnum = (await toSolution.GetEnumsAsync())
                        .FirstOrDefault(e => e.Id == @enum.Id);
                    oldEnumsToUpgrade.Add(toEnum);
                    newEnumsToUpgrade.Add(newEnum);
                }
            }

            if (oldEnumsToUpgrade.Count > 0)
            {
                var oldGenerateInfos = oldEnumsToUpgrade
                    .SelectMany(oldEnum => new[] {
                        new EnumModel(oldEnum, toSolution).GetGenerateInfo(),
                        new EnumViewModel(new View(), oldEnum, toSolution).GetGenerateInfo()
                    });
                var newGenerateInfos = newEnumsToUpgrade
                    .SelectMany(newEnum => new[] {
                        new EnumModel(newEnum, toSolution).GetGenerateInfo(),
                        new EnumViewModel(new View(), newEnum, toSolution).GetGenerateInfo()
                    });
                await UpgradeAsync(toSolution, oldGenerateInfos, newGenerateInfos);
            }
        }

        internal async Task<string> ReadFileAsync(string filePath)
        {
            using (var fileReader = new StreamReader(filePath))
            {
                return await fileReader.ReadToEndAsync();
            }
        }

        internal async Task WriteFileAsync(string filePath, string content)
        {
            using (var fileWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await fileWriter.WriteAsync(content);
            }
        }

        public async Task GenerateEntityAsync(SolutionAccessor solution, Entity entity)
        {
            var model = new EntityModel(entity, solution);
            await GenerateAsync(solution, model.GetGenerateInfo());
        }

        public async Task UpgradeEntityAsync(SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            await UpgradeEntitiesAsync(solution, new[] { oldEntity }, new[] { newEntity });
        }

        public async Task UpgradeEntitiesAsync(SolutionAccessor solution, IEnumerable<Entity> oldEntities, IEnumerable<Entity> newEntities)
        {
            var oldGenerateInfos = oldEntities
                .Select(oldEntity => new EntityModel(oldEntity, solution))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEntities
                .Select(newEntity => new EntityModel(newEntity, solution))
                .Select(model => model.GetGenerateInfo());

            await UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        public async Task GenerateEntityViewAsync(SolutionAccessor solution, Entity entity)
        {
            var views = await solution.GetViewsAsync();
            foreach (var view in views)
            {
                var model = new EntityViewModel(view, entity, solution);
                await GenerateAsync(solution, model.GetGenerateInfo());
            }
        }

        public async Task UpgradeEntityViewAsync(SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            await UpgradeEntityViewsAsync(solution, new[] { oldEntity }, new[] { newEntity });
        }

        public async Task UpgradeEntityViewsAsync(SolutionAccessor solution, IEnumerable<Entity> oldEntities, IEnumerable<Entity> newEntities)
        {
            var views = await solution.GetViewsAsync();
            var oldGenerateInfos = oldEntities
                .SelectMany(oldEntity => views.Select(v => new EntityViewModel(v, oldEntity, solution)))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEntities
                .SelectMany(newEntity => views.Select(v => new EntityViewModel(v, newEntity, solution)))
                .Select(model => model.GetGenerateInfo());

            await UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        public async Task GenerateEnumAsync(SolutionAccessor solution, Models.Enum @enum)
        {
            var model = new EnumModel(@enum, solution);
            await GenerateAsync(solution, model.GetGenerateInfo());
        }

        public async Task UpgradeEnumAsync(SolutionAccessor solution, Models.Enum oldEnum, Models.Enum newEnum)
        {
            await UpgradeEnumsAsync(solution, new[] { oldEnum }, new[] { newEnum });
        }

        public async Task UpgradeEnumsAsync(SolutionAccessor solution, IEnumerable<Models.Enum> oldEnums, IEnumerable<Models.Enum> newEnums)
        {
            var oldGenerateInfos = oldEnums
                .Select(oldEnum => new EnumModel(oldEnum, solution))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEnums
                .Select(newEnum => new EnumModel(newEnum, solution))
                .Select(model => model.GetGenerateInfo());

            await UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        public async Task GenerateEnumViewAsync(SolutionAccessor solution, Models.Enum @enum)
        {
            var views = await solution.GetViewsAsync();
            foreach (var view in views)
            {
                var model = new EnumViewModel(view, @enum, solution);
                await GenerateAsync(solution, model.GetGenerateInfo());
            }
        }

        public async Task UpgradeEnumViewAsync(SolutionAccessor solution, Models.Enum oldEnum, Models.Enum newEnum)
        {
            await UpgradeEnumViewsAsync(solution, new[] { oldEnum }, new[] { newEnum });
        }

        public async Task UpgradeEnumViewsAsync(SolutionAccessor solution, IEnumerable<Models.Enum> oldEnums, IEnumerable<Models.Enum> newEnums)
        {
            var views = await solution.GetViewsAsync();
            var oldGenerateInfos = oldEnums
                .SelectMany(oldEnum => views.Select(v => new EnumViewModel(v, oldEnum, solution)))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEnums
                .SelectMany(newEnum => views.Select(v => new EnumViewModel(v, newEnum, solution)))
                .Select(model => model.GetGenerateInfo());

            await UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        private async Task GenerateAsync(SolutionAccessor solution, GenerateInfo generateInfo, string overrideSolutionPath = null)
        {
            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.CheckTypes(generateInfo.Types))
                .Where(t => t.CheckView(generateInfo.View))
                .Where(t => t.CheckTargets(generateInfo.GenerationObject.Properties)))
            {
                await CreateFileAsync(template, generateInfo.Model, solution, generateInfo.GenerationObject, overrideSolutionPath);
            }
        }

        private async Task UpgradeAsync(SolutionAccessor solution,
            IEnumerable<GenerateInfo> oldGenerateInfos, IEnumerable<GenerateInfo> newGenerateInfos)
        {
            var oldPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var newPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(oldPath);
            Directory.CreateDirectory(newPath);

            foreach (var oldGenerateInfo in oldGenerateInfos)
            {
                await GenerateAsync(solution, oldGenerateInfo, oldPath);
            }
            foreach (var newGenerateInfo in newGenerateInfos) 
            {
                await GenerateAsync(solution, newGenerateInfo, newPath);
            }

            await _directoryMerge.MergeDirectoryAsync(oldPath, newPath, solution.Path);

            Directory.Delete(oldPath, true);
            Directory.Delete(newPath, true);
        }

        private bool HasWildcards(string path)
        {
            return path.Contains("*");
        }

        internal string FindDirectory(string path, string templatePath)
        {
            var templatePathItems = templatePath.Split(Path.DirectorySeparatorChar);
            foreach (var templatePathItem in templatePathItems)
            {
                var dir = Directory.GetDirectories(path, templatePathItem)
                    .FirstOrDefault();
                if (dir == null)
                {
                    if (HasWildcards(templatePathItem))
                        throw new Exception($"Can't find directory {templatePath}");
                    dir = Path.Combine(path, templatePathItem);
                    Directory.CreateDirectory(dir);
                }

                path = dir;
            }

            return path;
        }
    }
}
