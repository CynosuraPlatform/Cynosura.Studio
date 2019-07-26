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
        private readonly IMerge _merge;
        private readonly FileMerge _fileMerge;
        private readonly ILogger<CodeGenerator> _logger;
        public CodeGenerator(ITemplateEngine templateEngine,
            IPackageFeed packageFeed,
            IMerge merge,
            FileMerge fileMerge,
            ILogger<CodeGenerator> logger)
        {
            _templateEngine = templateEngine;
            _packageFeed = packageFeed;
            _merge = merge;
            _fileMerge = fileMerge;
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

        private async Task CreateFileAsync(CodeTemplate template, object model, SolutionAccessor solution, IGenerationObject generationObject)
        {
            var filePath = GetTemplateFilePath(template, solution, generationObject);
            var content = ProcessTemplate(template, solution, model);

            if (!string.IsNullOrEmpty(template.InsertAfter))
            {
                var fileContent = await ReadFileAsync(filePath);

                if (!fileContent.Contains(content))
                {
                    fileContent = fileContent.Replace(template.InsertAfter + Environment.NewLine,
                        template.InsertAfter + Environment.NewLine + content + Environment.NewLine);

                    await WriteFileAsync(filePath, fileContent);
                }
            }
            else
            {
                await WriteFileAsync(filePath, content);
            }
        }

        private async Task UpgradeFileAsync(CodeTemplate template, object oldModel, object newModel, SolutionAccessor solution, IGenerationObject oldGenerationObject, IGenerationObject newGenerationObject)
        {
            var oldContent = ProcessTemplate(template, solution, oldModel);
            var newContent = ProcessTemplate(template, solution, newModel);
            var oldFilePath = GetTemplateFilePath(template, solution, oldGenerationObject);
            var newFilePath = GetTemplateFilePath(template, solution, newGenerationObject);
            if (!File.Exists(oldFilePath))
            {
                _logger.LogWarning($"File {oldFilePath} is not found. Skip upgrade");
                return;
            }
            var oldFileContent = await ReadFileAsync(oldFilePath);
            var newFileContent = _merge.Merge(oldContent, newContent, oldFileContent);
            if (oldFilePath != newFilePath)
            {
                File.Delete(oldFilePath);
            }
            if (oldFileContent == newFileContent && oldFilePath == newFilePath)
                return;
            await WriteFileAsync(newFilePath, newFileContent);
        }

        private async Task RemoveFileAsync(CodeTemplate template, object model, SolutionAccessor solution, IGenerationObject generationObject)
        {
            var filePath = GetTemplateFilePath(template, solution, generationObject);
            if (!File.Exists(filePath))
                return;
            var content = ProcessTemplate(template, solution, model);
            if (!string.IsNullOrEmpty(template.InsertAfter))
            {
                var fileContent = await ReadFileAsync(filePath);
                fileContent = fileContent.Replace(content + Environment.NewLine, "");

                await WriteFileAsync(filePath, fileContent);
            }
            else
            {
                File.Delete(filePath);
            }
        }

        public async Task GenerateSolutionAsync(string path, string name, string templateName)
        {
            _logger.LogInformation("GenerateSolution");
            var latestVersion = (await _packageFeed.GetVersionsAsync(templateName)).First();
            _logger.LogInformation($"Latest version: {latestVersion}");
            if (Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0)
            {
                _logger.LogWarning($"Path {path} is not empty. Skipping solution generation");
                return;
            }
            await InitSolutionAsync(name, path, templateName, latestVersion);
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
                    await UpgradeEntityAsync(toSolution, toEntity, newEntity);
                    await UpgradeViewAsync(toSolution, new View(), toEntity, newEntity);
                }
            }
        }

        private async Task CopyEnumsAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
        {
            var fromEnums = await fromSolution.GetEnumsAsync();
            var toEnums = await toSolution.GetEnumsAsync();
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
                    await UpgradeEnumAsync(toSolution, toEnum, newEnum);
                    await UpgradeEnumViewAsync(toSolution, new View(), toEnum, newEnum);
                }
            }
        }

        public async Task UpgradeSolutionAsync(SolutionAccessor solution)
        {
            _logger.LogInformation("UpgradeSolution");
            if (solution.Metadata == null)
            {
                _logger.LogWarning("Solution metadata not found. Cannot upgrade.");
                return;
            }
            _logger.LogInformation($"Current version: {solution.Metadata.TemplateVersion}");
            var latestVersion = (await _packageFeed.GetVersionsAsync(solution.Metadata.TemplateName)).First();
            _logger.LogInformation($"Latest version: {latestVersion}");
            if (solution.Metadata.TemplateVersion == latestVersion)
            {
                _logger.LogWarning("Using latest version. Nothing to upgrade");
                return;
            }

            var solutionsPath = Path.Combine(StudioDirectoryPath, "Solutions");
            var latestPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{latestVersion}");
            if (Directory.Exists(latestPackageSolutionPath))
                Directory.Delete(latestPackageSolutionPath, true);
            var currentPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{solution.Metadata.TemplateVersion}");
            if (Directory.Exists(currentPackageSolutionPath))
                Directory.Delete(currentPackageSolutionPath, true);

            await InitSolutionAsync(solution.Namespace, latestPackageSolutionPath, solution.Metadata.TemplateName, latestVersion);
            var latestPackageSolution = new SolutionAccessor(latestPackageSolutionPath);
            await CopyEnumsAsync(solution, latestPackageSolution);
            await CopyEntitiesAsync(solution, latestPackageSolution);

            await InitSolutionAsync(solution.Namespace, currentPackageSolutionPath, solution.Metadata.TemplateName, solution.Metadata.TemplateVersion);
            var currentPackageSolution = new SolutionAccessor(currentPackageSolutionPath);
            await CopyEnumsAsync(solution, currentPackageSolution);
            await CopyEntitiesAsync(solution, currentPackageSolution);

            var upgradeRenames = await GetUpgradeRenames(currentPackageSolution, latestPackageSolution);
            RenameInSolution(solution, upgradeRenames);

            var templateResultRenames = await GetTemplateResultRenames(currentPackageSolution, latestPackageSolution);
            var renames = upgradeRenames.Concat(templateResultRenames).ToList();

            _logger.LogInformation($"Merging changes to {solution.Path}");
            await _fileMerge.MergeDirectoryAsync(currentPackageSolutionPath, latestPackageSolutionPath, solution.Path,
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
            var types = entity.GetTemplateTypes();
            await GenerateAsync(solution, entity, new EntityModel(entity, solution), types);
        }

        public async Task UpgradeEntityAsync(SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            var oldModel = new EntityModel(oldEntity, solution);
            var newModel = new EntityModel(newEntity, solution);

            var oldTypes = oldEntity.GetTemplateTypes();
            var newTypes = newEntity.GetTemplateTypes();

            await UpgradeAsync(solution, oldEntity, newEntity, oldModel, newModel, oldTypes, newTypes);
        }

        public async Task GenerateViewAsync(SolutionAccessor solution, View view, Entity entity)
        {
            var types = entity.GetViewTemplateTypes();
            await GenerateAsync(solution, entity, new ViewModel(view, entity, solution), types);
        }

        public async Task UpgradeViewAsync(SolutionAccessor solution, View view, Entity oldEntity, Entity newEntity)
        {
            var oldModel = new ViewModel(view, oldEntity, solution);
            var newModel = new ViewModel(view, newEntity, solution);

            var oldTypes = oldEntity.GetViewTemplateTypes();
            var newTypes = newEntity.GetViewTemplateTypes();

            await UpgradeAsync(solution, oldEntity, newEntity, oldModel, newModel, oldTypes, newTypes);
        }

        public async Task GenerateEnumAsync(SolutionAccessor solution, Models.Enum @enum)
        {
            await GenerateAsync(solution, @enum, new EnumModel(@enum, solution), @enum.GetTemplateTypes());
        }

        public async Task UpgradeEnumAsync(SolutionAccessor solution, Models.Enum oldEnum, Models.Enum newEnum)
        {
            var oldModel = new EnumModel(oldEnum, solution);
            var newModel = new EnumModel(newEnum, solution);

            var oldTypes = oldEnum.GetTemplateTypes();
            var newTypes = newEnum.GetTemplateTypes();

            await UpgradeAsync(solution, oldEnum, newEnum, oldModel, newModel, oldTypes, newTypes);
        }

        public async Task GenerateEnumViewAsync(SolutionAccessor solution, View view, Models.Enum @enum)
        {
            await GenerateAsync(solution, @enum, new EnumViewModel(view, @enum, solution), @enum.GetViewTemplateTypes());
        }

        public async Task UpgradeEnumViewAsync(SolutionAccessor solution, View view, Models.Enum oldEnum, Models.Enum newEnum)
        {
            var oldModel = new EnumViewModel(view, oldEnum, solution);
            var newModel = new EnumViewModel(view, newEnum, solution);

            var oldTypes = oldEnum.GetViewTemplateTypes();
            var newTypes = newEnum.GetViewTemplateTypes();

            await UpgradeAsync(solution, oldEnum, newEnum, oldModel, newModel, oldTypes, newTypes);
        }

        private async Task GenerateAsync(SolutionAccessor solution, IGenerationObject generationObject, object model, IEnumerable<TemplateType> types)
        {
            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.CheckTypes(types))
                .Where(t => t.CheckTargets(generationObject.Properties)))
            {
                await CreateFileAsync(template, model, solution, generationObject);
            }
        }

        private async Task UpgradeAsync(SolutionAccessor solution, 
            IGenerationObject oldGenerationObject, IGenerationObject newGenerationObject, 
            object oldModel, object newModel, 
            IEnumerable<TemplateType> oldTypes, IEnumerable<TemplateType> newTypes)
        {
            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.CheckTypes(oldTypes) || t.CheckTypes(newTypes)))
            {
                var oldExists = template.CheckTypes(oldTypes) && template.CheckTargets(oldGenerationObject.Properties);
                var newExists = template.CheckTypes(newTypes) && template.CheckTargets(newGenerationObject.Properties);
                if (oldExists && newExists)
                {
                    await UpgradeFileAsync(template, oldModel, newModel, solution, oldGenerationObject, newGenerationObject);
                }
                else if (oldExists)
                {
                    await RemoveFileAsync(template, oldModel, solution, oldGenerationObject);
                }
                else if (newExists)
                {
                    await CreateFileAsync(template, newModel, solution, newGenerationObject);
                }
            }
        }

        private bool HasWildcards(string path)
        {
            return path.Contains("*");
        }

        private string FindDirectory(string path, string templatePath)
        {
            var ignoreList = new[] { "AebIt.Platform.Common" };
            var templatePathItems = templatePath.Split(Path.DirectorySeparatorChar);
            foreach (var templatePathItem in templatePathItems)
            {
                var dir = Directory.GetDirectories(path, templatePathItem)
                    .FirstOrDefault(d => ignoreList.All(l => !d.Contains(l)));
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
