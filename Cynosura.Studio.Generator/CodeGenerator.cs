using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private string StudioDirectoryPath => Path.Combine(Path.GetTempPath(), "Cynosura.Studio");

        private readonly ITemplateEngine _templateEngine;
        private readonly IPackageFeed _packageFeed;
        private readonly IDirectoryMerge _directoryMerge;
        private readonly ILogger<CodeGenerator> _logger;
        public CodeGenerator(ITemplateEngine templateEngine,
            IPackageFeed packageFeed,
            IDirectoryMerge directoryMerge,
            ILogger<CodeGenerator> logger)
        {
            _templateEngine = templateEngine;
            _packageFeed = packageFeed;
            _directoryMerge = directoryMerge;
            _logger = logger;
        }

        private string ProcessTemplate(CodeTemplate template, SolutionAccessor solutionAccessor, object model)
        {
            var templatePath = solutionAccessor.GetTemplatePath(template);
            return _templateEngine.ProcessTemplate(templatePath, model);
        }

        private string GetTemplateFilePath(CodeTemplate template, SolutionAccessor solution, IGenerationObject generationObject)
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

        public async Task GenerateSolutionAsync(string path, string name, string templateName, string templateVersion = null)
        {
            _logger.LogInformation($"GenerateSolution {templateName} {templateVersion}");
            if (string.IsNullOrEmpty(templateVersion))
            {
                templateVersion = (await _packageFeed.GetVersionsAsync(templateName)).First();
            }
            _logger.LogInformation($"Use template: {templateName} {templateVersion}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0)
            {
                _logger.LogWarning($"Path {path} is not empty. Skipping solution generation");
                return;
            }
            await InitSolutionAsync(name, path, templateName, templateVersion);
        }

        private async Task InitSolutionAsync(string solutionName, string path, string templateName, string templateVersion)
        {
            var packagesPath = Path.Combine(StudioDirectoryPath, "Packages");
            if (!Directory.Exists(packagesPath))
                Directory.CreateDirectory(packagesPath);
            var packageFilePath = await _packageFeed.DownloadPackageAsync(packagesPath, templateName, templateVersion);
            _logger.LogInformation($"Downloaded version {templateVersion} to {packageFilePath}");

            CopyDirectory(packageFilePath, path);
            await RenameSolutionAsync(path, templateName, solutionName);
            _logger.LogInformation($"Created solution in {path}");
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

        private async Task CopyEntitiesAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
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
                    await GenerateViewAsync(toSolution, new View(), newEntity);
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
                        new ViewModel(new View(), oldEntity, toSolution).GetGenerateInfo()
                    });
                var newGenerateInfos = newEntitiesToUpgrade
                    .SelectMany(newEntity => new[] {
                        new EntityModel(newEntity, toSolution).GetGenerateInfo(),
                        new ViewModel(new View(), newEntity, toSolution).GetGenerateInfo()
                    });
                await UpgradeAsync(toSolution, oldGenerateInfos, newGenerateInfos);
            }
        }

        private async Task CopyEnumsAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
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
                    await GenerateEnumViewAsync(toSolution, new View(), newEnum);
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

        public async Task UpgradeSolutionAsync(SolutionAccessor solution, string templateName = null, string templateVersion = null)
        {
            _logger.LogInformation($"UpgradeSolution {templateName} {templateVersion}");
            if (solution.Metadata == null)
            {
                _logger.LogWarning("Solution metadata not found. Cannot upgrade.");
                return;
            }
            _logger.LogInformation($"Current version: {solution.Metadata.TemplateName} {solution.Metadata.TemplateVersion}");
            if (string.IsNullOrEmpty(templateName))
            {
                templateName = solution.Metadata.TemplateName;
            }
            if (string.IsNullOrEmpty(templateVersion))
            {
                templateVersion = (await _packageFeed.GetVersionsAsync(templateName)).First();
            }
            _logger.LogInformation($"Upgrade version: {templateName} {templateVersion}");
            if (solution.Metadata.TemplateName == templateName && solution.Metadata.TemplateVersion == templateVersion)
            {
                _logger.LogWarning("Using latest version. Nothing to upgrade");
                return;
            }

            var solutionsPath = Path.Combine(StudioDirectoryPath, "Solutions");
            var upgradePackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{templateName}.{templateVersion}");
            if (Directory.Exists(upgradePackageSolutionPath))
                Directory.Delete(upgradePackageSolutionPath, true);
            var currentPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{solution.Metadata.TemplateName}.{solution.Metadata.TemplateVersion}");
            if (Directory.Exists(currentPackageSolutionPath))
                Directory.Delete(currentPackageSolutionPath, true);

            await InitSolutionAsync(solution.Namespace, upgradePackageSolutionPath, templateName, templateVersion);
            var upgradePackageSolution = new SolutionAccessor(upgradePackageSolutionPath);
            await CopyEnumsAsync(solution, upgradePackageSolution);
            await CopyEntitiesAsync(solution, upgradePackageSolution);

            await InitSolutionAsync(solution.Namespace, currentPackageSolutionPath, solution.Metadata.TemplateName, solution.Metadata.TemplateVersion);
            var currentPackageSolution = new SolutionAccessor(currentPackageSolutionPath);
            await CopyEnumsAsync(solution, currentPackageSolution);
            await CopyEntitiesAsync(solution, currentPackageSolution);

            var upgradeRenames = await GetUpgradeRenames(currentPackageSolution, upgradePackageSolution);
            RenameInSolution(solution, upgradeRenames);

            var templateResultRenames = await GetTemplateResultRenames(currentPackageSolution, upgradePackageSolution);
            var renames = upgradeRenames.Concat(templateResultRenames).ToList();

            _logger.LogInformation($"Merging changes to {solution.Path}");
            await _directoryMerge.MergeDirectoryAsync(currentPackageSolutionPath, upgradePackageSolutionPath, solution.Path,
                renames);
            _logger.LogInformation($"Completed");
        }

        private void RenameInSolution(SolutionAccessor solution, IList<(string Left, string Right)> renames)
        {
            foreach (var rename in renames)
            {
                var left = Path.Combine(solution.Path, rename.Left);
                var path = Path.Combine(solution.Path, rename.Right);
                Directory.Move(left, path);
            }
        }

        private async Task<IList<(string, string)>> GetUpgradeRenames(SolutionAccessor sourceSolution, SolutionAccessor destinationSolution)
        {
            var renames = new List<(string, string)>();

            var sourceUpgrade = await sourceSolution.GetUpgradeMetadataAsync();
            var destinationUpgrade = await destinationSolution.GetUpgradeMetadataAsync();
            var sourceVersion = sourceUpgrade.Version;

            while (true)
            {
                if (sourceVersion == destinationUpgrade.Version)
                    break;
                var version = sourceVersion;
                var upgrade = destinationUpgrade.Upgrades.FirstOrDefault(u => u.From == version);
                if (upgrade == null)
                    throw new ServiceException("Can't upgrade template");
                foreach (var upgradeRename in upgrade.Renames)
                {
                    var left = FindDirectory(destinationSolution.Path, upgradeRename.Left);
                    left = Path.GetRelativePath(destinationSolution.Path, left);
                    var right = FindDirectory(destinationSolution.Path, upgradeRename.Right);
                    right = Path.GetRelativePath(destinationSolution.Path, right);
                    renames.Add((left, right));
                }
                sourceVersion = upgrade.To;
            }

            return renames;
        }

        private async Task<IList<(string, string)>> GetTemplateResultRenames(SolutionAccessor sourceSolution, SolutionAccessor destinationSolution)
        {
            var renames = new List<(string, string)>();

            var sourceTemplates = (await sourceSolution.LoadTemplatesAsync()).ToList();
            var destinationTemplates = (await destinationSolution.LoadTemplatesAsync()).ToList();

            var sourceEntities = await sourceSolution.GetEntitiesAsync();
            var destinationEntities = await destinationSolution.GetEntitiesAsync();

            var sourceEnums = await sourceSolution.GetEnumsAsync();
            var destinationEnums = await destinationSolution.GetEnumsAsync();

            foreach (var sourceTemplate in sourceTemplates)
            {
                var destinationTemplate = destinationTemplates.FirstOrDefault(t => t.TemplatePath == sourceTemplate.TemplatePath);
                if (destinationTemplate == null)
                    continue;
                if (sourceTemplate.Types.Contains(TemplateType.Entity) || sourceTemplate.Types.Contains(TemplateType.View))
                {
                    foreach (var sourceEntity in sourceEntities)
                    {
                        var destinationEntity = destinationEntities.FirstOrDefault(e => e.Id == sourceEntity.Id);
                        if (destinationEntity == null)
                            continue;
                        var sourceFilePath = GetTemplateFilePath(sourceTemplate, sourceSolution, sourceEntity);
                        var destinationFilePath = GetTemplateFilePath(destinationTemplate, destinationSolution, destinationEntity);
                        sourceFilePath = Path.GetRelativePath(sourceSolution.Path, sourceFilePath);
                        destinationFilePath = Path.GetRelativePath(destinationSolution.Path, destinationFilePath);
                        if (sourceFilePath != destinationFilePath)
                            renames.Add((sourceFilePath, destinationFilePath));
                    }
                }
                else if (sourceTemplate.Types.Contains(TemplateType.Enum) || sourceTemplate.Types.Contains(TemplateType.EnumView))
                {
                    foreach (var sourceEnum in sourceEnums)
                    {
                        var destinationEnum = destinationEnums.FirstOrDefault(e => e.Id == sourceEnum.Id);
                        if (destinationEnum == null)
                            continue;
                        var sourceFilePath = GetTemplateFilePath(sourceTemplate, sourceSolution, sourceEnum);
                        var destinationFilePath = GetTemplateFilePath(destinationTemplate, destinationSolution, destinationEnum);
                        sourceFilePath = Path.GetRelativePath(sourceSolution.Path, sourceFilePath);
                        destinationFilePath = Path.GetRelativePath(destinationSolution.Path, destinationFilePath);
                        if (sourceFilePath != destinationFilePath)
                            renames.Add((sourceFilePath, destinationFilePath));
                    }
                }
            }

            return renames;
        }

        private void CopyDirectory(string fromPath, string toPath)
        {
            foreach (string dirPath in Directory.GetDirectories(fromPath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(fromPath, toPath));

            foreach (string newPath in Directory.GetFiles(fromPath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(fromPath, toPath), true);
        }

        private async Task RenameSolutionAsync(string path, string oldValue, string newValue)
        {
            foreach (var directory in Directory.GetDirectories(path))
            {
                var directoryName = Path.GetRelativePath(path, directory);
                var newDirectoryName = directoryName.Replace(oldValue, newValue);
                var newDirectory = Path.Combine(path, newDirectoryName);
                if (directory != newDirectory)
                {
                    if (Directory.Exists(newDirectory))
                        Directory.Delete(newDirectory);
                    Directory.Move(directory, newDirectory);
                }
                await RenameSolutionAsync(newDirectory, oldValue, newValue);
            }

            foreach (var file in Directory.GetFiles(path))
            {
                var fileName = Path.GetRelativePath(path, file);
                var newFileName = fileName.Replace(oldValue, newValue);
                var newFile = Path.Combine(path, newFileName);
                if (file != newFile)
                {
                    Directory.Move(file, newFile);
                }

                var fileContent = await ReadFileAsync(newFile);
                var newFileContent = fileContent.Replace(oldValue, newValue);
                if (fileContent != newFileContent)
                {
                    await WriteFileAsync(newFile, newFileContent);
                }
            }
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

        public async Task GenerateViewAsync(SolutionAccessor solution, View view, Entity entity)
        {
            var model = new ViewModel(view, entity, solution);
            await GenerateAsync(solution, model.GetGenerateInfo());
        }

        public async Task UpgradeViewAsync(SolutionAccessor solution, View view, Entity oldEntity, Entity newEntity)
        {
            await UpgradeViewsAsync(solution, view, new[] { oldEntity }, new[] { newEntity });
        }

        public async Task UpgradeViewsAsync(SolutionAccessor solution, View view, IEnumerable<Entity> oldEntities, IEnumerable<Entity> newEntities)
        {
            var oldGenerateInfos = oldEntities
                .Select(oldEntity => new ViewModel(view, oldEntity, solution))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEntities
                .Select(newEntity => new ViewModel(view, newEntity, solution))
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

        public async Task GenerateEnumViewAsync(SolutionAccessor solution, View view, Models.Enum @enum)
        {
            var model = new EnumViewModel(view, @enum, solution);
            await GenerateAsync(solution, model.GetGenerateInfo());
        }

        public async Task UpgradeEnumViewAsync(SolutionAccessor solution, View view, Models.Enum oldEnum, Models.Enum newEnum)
        {
            await UpgradeEnumViewsAsync(solution, view, new[] { oldEnum }, new[] { newEnum });
        }

        public async Task UpgradeEnumViewsAsync(SolutionAccessor solution, View view, IEnumerable<Models.Enum> oldEnums, IEnumerable<Models.Enum> newEnums)
        {
            var oldGenerateInfos = oldEnums
                .Select(oldEnum => new EnumViewModel(view, oldEnum, solution))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEnums
                .Select(newEnum => new EnumViewModel(view, newEnum, solution))
                .Select(model => model.GetGenerateInfo());

            await UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        private async Task GenerateAsync(SolutionAccessor solution, GenerateInfo generateInfo, string overrideSolutionPath = null)
        {
            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.CheckTypes(generateInfo.Types))
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

        private string FindDirectory(string path, string templatePath)
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
