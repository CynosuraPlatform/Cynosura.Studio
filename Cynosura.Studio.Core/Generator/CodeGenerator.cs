using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cynosura.Studio.Core.Generator.Models;
using Cynosura.Studio.Core.TemplateEngine;

namespace Cynosura.Studio.Core.Generator
{
    public class CodeGenerator
    {
        private readonly ITemplateEngine _templateEngine;
        public IList<CodeTemplate> Templates { get; set; } = new List<CodeTemplate>();

        public CodeGenerator(ITemplateEngine templateEngine)
        {
            _templateEngine = templateEngine;
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
        }

        private void CreateFile(CodeTemplate template, object model, Solution solution, Entity entity)
        {
            var dir = FindDirectory(solution.Path, template.FilePath);
            var fileName = ProcessFileName(template.FileName, entity);
            var filePath = Path.Combine(dir, fileName);
            var fileDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
            var templatePath = PathHelper.GetAbsolutePath("..\\..\\..\\..\\Cynosura.Studio.Core\\Templates", template.TemplatePath);
            var content = _templateEngine.ProcessTemplate(templatePath, model);

            if (!string.IsNullOrEmpty(template.InsertAfter))
            {
                string fileContent;
                using (var streamWriter = new StreamReader(filePath))
                {
                    fileContent = streamWriter.ReadToEnd();
                }

                if (!fileContent.Contains(content))
                {
                    fileContent = fileContent.Replace(template.InsertAfter + Environment.NewLine,
                        template.InsertAfter + Environment.NewLine + content + Environment.NewLine);

                    using (var streamWriter = new StreamWriter(filePath))
                    {
                        streamWriter.Write(fileContent);
                    }
                }
            }
            else
            {
                using (var streamWriter = new StreamWriter(filePath))
                {
                    streamWriter.Write(content);
                }
            }
        }

        public void GenerateSolution(string path, string name)
        {
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

        public void GenerateEntity(Solution solution, Entity entity)
        {
            var model = new EntityModel()
            {
                Entity = entity,
                Solution = solution,
            };

            foreach (var template in Templates.Where(t => t.Type == TemplateType.Entity))
            {
                CreateFile(template, model, solution, entity);
            }
        }

        public void GenerateView(Solution solution, View view, Entity entity)
        {
            var model = new ViewModel()
            {
                View = view,
                Entity = entity,
                Solution = solution,
            };

            foreach (var template in Templates.Where(t => t.Type == TemplateType.View))
            {
                CreateFile(template, model, solution, entity);
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
