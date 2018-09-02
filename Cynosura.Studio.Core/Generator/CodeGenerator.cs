using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Core.Generator.Models;
using Cynosura.Studio.Core.Merge;
using Cynosura.Studio.Core.PackageFeed;
using Cynosura.Studio.Core.TemplateEngine;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Core.Generator
{
    public class CodeGenerator
    {
        private const string PackageName = "Cynosura.Template";
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

        private string GetTemplateFilePath(CodeTemplate template, SolutionAccessor solution, Entity entity)
        {
            var dir = FindDirectory(solution.Path, template.FilePath);
            var fileName = ProcessFileName(template.FileName, entity);
            var filePath = Path.Combine(dir, fileName);
            var fileDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
            return filePath;
        }

        private async Task CreateFileAsync(CodeTemplate template, object model, SolutionAccessor solution, Entity entity)
        {
            var filePath = GetTemplateFilePath(template, solution, entity);
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

        private async Task UpgradeFileAsync(CodeTemplate template, object oldModel, object newModel, SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            var oldContent = ProcessTemplate(template, solution, oldModel);
            var newContent = ProcessTemplate(template, solution, newModel);
            var oldFilePath = GetTemplateFilePath(template, solution, oldEntity);
            var newFilePath = GetTemplateFilePath(template, solution, newEntity);
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

        public async Task GenerateSolutionAsync(string path, string name)
        {
            _logger.LogInformation("GenerateSolution");
            var latestVersion = (await _packageFeed.GetVersionsAsync(PackageName)).First();
            _logger.LogInformation($"Latest version: {latestVersion}");
            if (Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0)
            {
                _logger.LogWarning($"Path {path} is not empty. Skipping solution generation");
                return;
            }
            await InitSolutionAsync(name, latestVersion, path);
        }

        private async Task InitSolutionAsync(string solutionName, string packageVersion, string path)
        {
            var packagesPath = Path.Combine(StudioDirectoryPath, "Packages");
            if (!Directory.Exists(packagesPath))
                Directory.CreateDirectory(packagesPath);
            var packageFilePath = await _packageFeed.DownloadPackageAsync(packagesPath, PackageName, packageVersion);
            _logger.LogInformation($"Downloaded version {packageVersion} to {packageFilePath}");

            CopyDirectory(packageFilePath, path);
            await RenameSolutionAsync(path, PackageName, solutionName);
            _logger.LogInformation($"Created solution in {path}");
        }

        private async Task CopyEntitiesAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
        {
            var entities = await fromSolution.GetEntitiesAsync();
            foreach (var entity in entities)
            {
                await toSolution.CreateEntityAsync(entity);
            }
        }

        private async Task GenerateAllEntitiesAsync(SolutionAccessor solution)
        {
            var entities = await solution.GetEntitiesAsync();
            foreach (var entity in entities)
            {
                await GenerateEntityAsync(solution, entity);
                await GenerateViewAsync(solution, new View(), entity);
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
            _logger.LogInformation($"Current version: {solution.Metadata.Version}");
            var latestVersion = (await _packageFeed.GetVersionsAsync(PackageName)).First();
            _logger.LogInformation($"Latest version: {latestVersion}");
            if (solution.Metadata.Version == latestVersion)
            {
                _logger.LogWarning("Using latest version. Nothing to upgrade");
                return;
            }

            var solutionsPath = Path.Combine(StudioDirectoryPath, "Solutions");
            var latestPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{latestVersion}");
            if (Directory.Exists(latestPackageSolutionPath))
                Directory.Delete(latestPackageSolutionPath, true);
            var currentPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{solution.Metadata.Version}");
            if (Directory.Exists(currentPackageSolutionPath))
                Directory.Delete(currentPackageSolutionPath, true);

            await InitSolutionAsync(solution.Namespace, latestVersion, latestPackageSolutionPath);
            var latestPackageSolution = new SolutionAccessor(latestPackageSolutionPath);
            await CopyEntitiesAsync(solution, latestPackageSolution);
            await GenerateAllEntitiesAsync(latestPackageSolution);

            await InitSolutionAsync(solution.Namespace, solution.Metadata.Version, currentPackageSolutionPath);
            var currentPackageSolution = new SolutionAccessor(currentPackageSolutionPath);
            await CopyEntitiesAsync(solution, currentPackageSolution);
            await GenerateAllEntitiesAsync(currentPackageSolution);

            _logger.LogInformation($"Merging changes to {solution.Path}");
            await _fileMerge.MergeDirectoryAsync(currentPackageSolutionPath, latestPackageSolutionPath, solution.Path);
            _logger.LogInformation($"Completed");
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
            var model = new EntityModel()
            {
                Entity = entity,
                Solution = solution,
            };

            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.Type == TemplateType.Entity))
            {
                await CreateFileAsync(template, model, solution, entity);
            }
        }

        public async Task UpgradeEntityAsync(SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            var oldModel = new EntityModel()
            {
                Entity = oldEntity,
                Solution = solution,
            };

            var newModel = new EntityModel()
            {
                Entity = newEntity,
                Solution = solution,
            };

            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.Type == TemplateType.Entity))
            {
                await UpgradeFileAsync(template, oldModel, newModel, solution, oldEntity, newEntity);
            }
        }

        public async Task GenerateViewAsync(SolutionAccessor solution, View view, Entity entity)
        {
            var model = new ViewModel()
            {
                View = view,
                Entity = entity,
                Solution = solution,
            };

            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.Type == TemplateType.View))
            {
                await CreateFileAsync(template, model, solution, entity);
            }
        }

        public async Task UpgradeViewAsync(SolutionAccessor solution, View view, Entity oldEntity, Entity newEntity)
        {
            var oldModel = new ViewModel()
            {
                View = view,
                Entity = oldEntity,
                Solution = solution,
            };

            var newModel = new ViewModel()
            {
                View = view,
                Entity = newEntity,
                Solution = solution,
            };

            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.Type == TemplateType.View))
            {
                await UpgradeFileAsync(template, oldModel, newModel, solution, oldEntity, newEntity);
            }
        }

        private string ProcessFileName(string fileName, Entity entity)
        {
            fileName = fileName.Replace("{Name}", entity.Name);
            fileName = fileName.Replace("{NameLower}", entity.NameLower);
            return fileName;
        }

        private bool HasWildcards(string path)
        {
            return path.Contains("*");
        }

        private string FindDirectory(string path, string templatePath)
        {
            var ignoreList = new List<string>() {"AebIt.Platform.Common"};
            var templatePathItems = templatePath.Split('\\');
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
