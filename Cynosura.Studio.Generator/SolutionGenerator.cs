using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Core.Services;
using Cynosura.Studio.Generator.Merge;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Generator.PackageFeed;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Generator
{
    public class SolutionGenerator
    {
        private string StudioDirectoryPath => Path.Combine(Path.GetTempPath(), "Cynosura.Studio");

        private readonly CodeGenerator _codeGenerator;
        private readonly EntityGenerator _entityGenerator;
        private readonly EnumGenerator _enumGenerator;
        private readonly IDirectoryMerge _directoryMerge;
        private readonly IPackageFeed _packageFeed;
        private readonly ILogger<SolutionGenerator> _logger;

        public SolutionGenerator(CodeGenerator codeGenerator,
            EntityGenerator entityGenerator,
            EnumGenerator enumGenerator,
            IDirectoryMerge directoryMerge,
            IPackageFeed packageFeed,
            ILogger<SolutionGenerator> logger)
        {
            _codeGenerator = codeGenerator;
            _entityGenerator = entityGenerator;
            _enumGenerator = enumGenerator;
            _directoryMerge = directoryMerge;
            _packageFeed = packageFeed;
            _logger = logger;
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

                var fileContent = await _codeGenerator.ReadFileAsync(newFile);
                var newFileContent = fileContent.Replace(oldValue, newValue);
                if (fileContent != newFileContent)
                {
                    await _codeGenerator.WriteFileAsync(newFile, newFileContent);
                }
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
            await _enumGenerator.CopyEnumsAsync(solution, upgradePackageSolution);
            await _entityGenerator.CopyEntitiesAsync(solution, upgradePackageSolution);

            await InitSolutionAsync(solution.Namespace, currentPackageSolutionPath, solution.Metadata.TemplateName, solution.Metadata.TemplateVersion);
            var currentPackageSolution = new SolutionAccessor(currentPackageSolutionPath);
            await _enumGenerator.CopyEnumsAsync(solution, currentPackageSolution);
            await _entityGenerator.CopyEntitiesAsync(solution, currentPackageSolution);

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
                    var left = _codeGenerator.FindDirectory(destinationSolution.Path, upgradeRename.Left);
                    left = Path.GetRelativePath(destinationSolution.Path, left);
                    var right = _codeGenerator.FindDirectory(destinationSolution.Path, upgradeRename.Right);
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
                if (sourceTemplate.Types.Contains(TemplateType.Entity))
                {
                    foreach (var sourceEntity in sourceEntities)
                    {
                        var destinationEntity = destinationEntities.FirstOrDefault(e => e.Id == sourceEntity.Id);
                        if (destinationEntity == null)
                            continue;
                        var sourceFilePath = _codeGenerator.GetTemplateFilePath(sourceTemplate, sourceSolution, sourceEntity);
                        var destinationFilePath = _codeGenerator.GetTemplateFilePath(destinationTemplate, destinationSolution, destinationEntity);
                        sourceFilePath = Path.GetRelativePath(sourceSolution.Path, sourceFilePath);
                        destinationFilePath = Path.GetRelativePath(destinationSolution.Path, destinationFilePath);
                        if (sourceFilePath != destinationFilePath)
                            renames.Add((sourceFilePath, destinationFilePath));
                    }
                }
                else if (sourceTemplate.Types.Contains(TemplateType.Enum))
                {
                    foreach (var sourceEnum in sourceEnums)
                    {
                        var destinationEnum = destinationEnums.FirstOrDefault(e => e.Id == sourceEnum.Id);
                        if (destinationEnum == null)
                            continue;
                        var sourceFilePath = _codeGenerator.GetTemplateFilePath(sourceTemplate, sourceSolution, sourceEnum);
                        var destinationFilePath = _codeGenerator.GetTemplateFilePath(destinationTemplate, destinationSolution, destinationEnum);
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
    }
}
