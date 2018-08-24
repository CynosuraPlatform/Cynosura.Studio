using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly ITemplateEngine _templateEngine;
        private readonly IPackageFeed _packageFeed;
        private readonly IMerge _merge;
        private readonly FileMerge _fileMerge;
        private readonly ILogger<CodeGenerator> _logger;
        public IList<CodeTemplate> Templates { get; set; } = new List<CodeTemplate>();

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
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\Entities", FileName = "{Name}.cs", TemplatePath = "Core\\Entity.stg"});
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\Services\\Models", FileName = "{Name}CreateModel.cs", TemplatePath = "Core\\ServiceCreateModel.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\Services\\Models", FileName = "{Name}UpdateModel.cs", TemplatePath = "Core\\ServiceUpdateModel.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\Services", FileName = "I{Name}Service.cs", TemplatePath = "Core\\ServiceInterface.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\Services", FileName = "{Name}Service.cs", TemplatePath = "Core\\Service.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\AutoMapper", FileName = "{Name}Profile.cs", TemplatePath = "Core\\AutoMapperProfile.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Core\\Autofac", FileName = "{Name}Module.cs", TemplatePath = "Core\\AutofacModule.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Data\\Autofac", FileName = "{Name}Module.cs", TemplatePath = "Data\\AutofacModule.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Data\\DataContextModule", FileName = "{Name}Module.cs", TemplatePath = "Data\\DataContextModule.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Models", FileName = "{Name}ViewModels\\{Name}CreateViewModel.cs", TemplatePath = "Web\\CreateViewModel.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Models", FileName = "{Name}ViewModels\\{Name}UpdateViewModel.cs", TemplatePath = "Web\\UpdateViewModel.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Models", FileName = "{Name}ViewModels\\{Name}ViewModel.cs", TemplatePath = "Web\\ViewModel.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Models", FileName = "{Name}ViewModels\\{Name}ShortViewModel.cs", TemplatePath = "Web\\ShortViewModel.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Controllers", FileName = "{Name}Controller.cs", TemplatePath = "Web\\Controller.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Infrastructure\\AutoMapper", FileName = "{Name}Profile.cs", TemplatePath = "Web\\AutoMapperProfile.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.Entity, FilePath = "*.Web\\Infrastructure\\Menu", FileName = "{Name}Module.cs", TemplatePath = "Web\\MenuModule.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}-core\\{NameLower}.model.ts", TemplatePath = "Web\\ClientApp\\Model.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}-core\\{NameLower}.service.ts", TemplatePath = "Web\\ClientApp\\Service.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}-core\\select.component.ts", TemplatePath = "Web\\ClientApp\\SelectComponent.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}-core\\select.component.html", TemplatePath = "Web\\ClientApp\\SelectView.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}-core\\{NameLower}-core.module.ts", TemplatePath = "Web\\ClientApp\\CoreModule.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}\\list.component.ts", TemplatePath = "Web\\ClientApp\\ListComponent.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}\\list.component.html", TemplatePath = "Web\\ClientApp\\ListView.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}\\edit.component.ts", TemplatePath = "Web\\ClientApp\\EditComponent.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}\\edit.component.html", TemplatePath = "Web\\ClientApp\\EditView.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "{NameLower}\\{NameLower}.module.ts", TemplatePath = "Web\\ClientApp\\Module.stg" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "app.module.ts", TemplatePath = "Web\\ClientApp\\Route.stg", InsertAfter = "// ADD ROUTES HERE" });
            Templates.Add(new CodeTemplate() { Type = TemplateType.View, FilePath = "*.Web\\ClientApp\\src\\app", FileName = "app.module.ts", TemplatePath = "Web\\ClientApp\\RouteImport.stg", InsertAfter = "// ADD MODULES HERE" });
        }

        private string ProcessTemplate(CodeTemplate template, object model)
        {
            var templatePath = PathHelper.GetAbsolutePath("..\\..\\..\\..\\Cynosura.Studio.Core\\Templates", template.TemplatePath);
            return _templateEngine.ProcessTemplate(templatePath, model);
        }

        private string GetTemplateFilePath(CodeTemplate template, Solution solution, Entity entity)
        {
            var dir = FindDirectory(solution.Path, template.FilePath);
            var fileName = ProcessFileName(template.FileName, entity);
            var filePath = Path.Combine(dir, fileName);
            var fileDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
            return filePath;
        }

        private async Task CreateFileAsync(CodeTemplate template, object model, Solution solution, Entity entity)
        {
            var filePath = GetTemplateFilePath(template, solution, entity);
            var content = ProcessTemplate(template, model);

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

        private async Task UpgradeFileAsync(CodeTemplate template, object oldModel, object newModel, Solution solution, Entity oldEntity, Entity newEntity)
        {
            var oldContent = ProcessTemplate(template, oldModel);
            var newContent = ProcessTemplate(template, newModel);
            var oldFilePath = GetTemplateFilePath(template, solution, oldEntity);
            var newFilePath = GetTemplateFilePath(template, solution, newEntity);
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

        public void GenerateSolution(string path, string name)
        {
            _logger.LogInformation("GenerateSolution");
            if (Directory.GetFiles(path).Any())
            {
                _logger.LogWarning($"Path {path} is not empty. Skip generation.");
                return;
            }
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = "new -i Cynosura.Template",
                WorkingDirectory = path
            });
            process.WaitForExit();
            process = Process.Start(new ProcessStartInfo()
            {
                FileName = "dotnet",
                Arguments = $"new cynosura -n {name}",
                WorkingDirectory = path
            });
            process.WaitForExit();
        }

        public async Task UpgradeSolutionAsync(Solution solution)
        {
            const string packageName = "Cynosura.Template";
            string studioDirectoryPath = Path.Combine(Path.GetTempPath(), "Cynosura.Studio");

            _logger.LogInformation("UpgradeSolution");
            if (solution.Metadata == null)
            {
                _logger.LogWarning("Solution metadata not found. Cannot upgrade.");
                return;
            }
            _logger.LogInformation($"Current version: {solution.Metadata.Version}");
            var latestVersion = (await _packageFeed.GetVersionsAsync(packageName)).First();
            _logger.LogInformation($"Latest version: {latestVersion}");
            if (solution.Metadata.Version == latestVersion)
            {
                _logger.LogWarning("Using latest version. Nothing to upgrade");
                return;
            }
            var packagesPath = Path.Combine(studioDirectoryPath, "Packages");
            if (!Directory.Exists(packagesPath))
                Directory.CreateDirectory(packagesPath);
            var latestPackageFilePath = await _packageFeed.DownloadPackageAsync(packagesPath, packageName, latestVersion);
            _logger.LogInformation($"Downloaded latest version to {latestPackageFilePath}");
            var currentPackageFilePath = await _packageFeed.DownloadPackageAsync(packagesPath, packageName, solution.Metadata.Version);
            _logger.LogInformation($"Downloaded current version to {currentPackageFilePath}");

            var solutionsPath = Path.Combine(studioDirectoryPath, "Solutions");
            var currentPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{solution.Metadata.Version}");
            if (Directory.Exists(currentPackageSolutionPath))
                Directory.Delete(currentPackageSolutionPath, true);
            CopyDirectory(currentPackageFilePath, currentPackageSolutionPath);
            await RenameSolutionAsync(currentPackageSolutionPath, packageName, solution.Namespace);
            _logger.LogInformation($"Created {currentPackageSolutionPath}");

            var latestPackageSolutionPath = Path.Combine(solutionsPath, $"{solution.Namespace}.{latestVersion}");
            if (Directory.Exists(latestPackageSolutionPath))
                Directory.Delete(latestPackageSolutionPath, true);
            CopyDirectory(latestPackageFilePath, latestPackageSolutionPath);
            await RenameSolutionAsync(latestPackageSolutionPath, packageName, solution.Namespace);
            _logger.LogInformation($"Created {latestPackageSolutionPath}");

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

        public async Task GenerateEntityAsync(Solution solution, Entity entity)
        {
            var model = new EntityModel()
            {
                Entity = entity,
                Solution = solution,
            };

            foreach (var template in Templates.Where(t => t.Type == TemplateType.Entity))
            {
                await CreateFileAsync(template, model, solution, entity);
            }
        }

        public async Task UpgradeEntityAsync(Solution solution, Entity oldEntity, Entity newEntity)
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

            foreach (var template in Templates.Where(t => t.Type == TemplateType.Entity))
            {
                await UpgradeFileAsync(template, oldModel, newModel, solution, oldEntity, newEntity);
            }
        }

        public async Task GenerateViewAsync(Solution solution, View view, Entity entity)
        {
            var model = new ViewModel()
            {
                View = view,
                Entity = entity,
                Solution = solution,
            };

            foreach (var template in Templates.Where(t => t.Type == TemplateType.View))
            {
                await CreateFileAsync(template, model, solution, entity);
            }
        }

        public async Task UpgradeViewAsync(Solution solution, View view, Entity oldEntity, Entity newEntity)
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

            foreach (var template in Templates.Where(t => t.Type == TemplateType.View))
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
