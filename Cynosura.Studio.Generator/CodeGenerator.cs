﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Models;
using Cynosura.Studio.Generator.Merge;
using Cynosura.Studio.Generator.TemplateEngine;

namespace Cynosura.Studio.Generator
{
    public class CodeGenerator
    {
        private readonly ITemplateEngine _templateEngine;
        private readonly IDirectoryMerge _directoryMerge;
        public CodeGenerator(ITemplateEngine templateEngine,
            IDirectoryMerge directoryMerge)
        {
            _templateEngine = templateEngine;
            _directoryMerge = directoryMerge;
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
                await FileHelper.InsertTextAsync(filePath, content, template.InsertAfter, fileSavePath);
            }
            else
            {
                await FileHelper.WriteFileAsync(fileSavePath, content);
            }
        }

        private async Task DeleteFileAsync(CodeTemplate template, object model, SolutionAccessor solution, IGenerationObject generationObject)
        {
            var filePath = GetTemplateFilePath(template, solution, generationObject);

            if (!string.IsNullOrEmpty(template.InsertAfter))
            {
                var content = ProcessTemplate(template, solution, model);
                await FileHelper.RemoveTextAsync(filePath, content);
            }
            else
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        internal async Task GenerateAsync(SolutionAccessor solution, GenerateInfo generateInfo, string overrideSolutionPath = null)
        {
            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.CheckTypes(generateInfo.Types))
                .Where(t => t.CheckView(generateInfo.View))
                .Where(t => t.CheckTargets(generateInfo.GenerationObject.Properties)))
            {
                await CreateFileAsync(template, generateInfo.Model, solution, generateInfo.GenerationObject, overrideSolutionPath);
            }
        }

        internal async Task UpgradeAsync(SolutionAccessor solution,
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

        internal async Task DeleteAsync(SolutionAccessor solution, GenerateInfo generateInfo)
        {
            var templates = await solution.LoadTemplatesAsync();
            foreach (var template in templates.Where(t => t.CheckTypes(generateInfo.Types))
                .Where(t => t.CheckView(generateInfo.View))
                .Where(t => t.CheckTargets(generateInfo.GenerationObject.Properties)))
            {
                await DeleteFileAsync(template, generateInfo.Model, solution, generateInfo.GenerationObject);
            }
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
