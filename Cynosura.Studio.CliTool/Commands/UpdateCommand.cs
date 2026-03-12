using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Generator;
using Cynosura.Studio.Generator.Infrastructure;
using Cynosura.Studio.Generator.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool.Commands
{
    public class UpdateCommand : AppCommand
    {
        private readonly Dictionary<string, Func<IEnumerable<string>, Task<bool>>> _actions;

        public UpdateCommand(string solutionDirectory, string feed, string src, string templateName, ServiceProvider serviceProvider)
            : base(solutionDirectory, feed, src, templateName, serviceProvider)
        {
            _actions = new Dictionary<string, Func<IEnumerable<string>, Task<bool>>>
            {
                {"entity", UpdateEntityActionAsync},
                {"enum", UpdateEnumActionAsync},
                {
                    "help", (_) =>
                    {
                        Console.Write(Help());
                        return Task.FromResult(true);
                    }
                }
            };
        }

        public override async Task<bool> ExecuteAsync(string[] args)
        {
            var command = args.FirstOrDefault();
            if (!string.IsNullOrEmpty(command) && _actions.ContainsKey(command))
            {
                return await _actions[command].Invoke(GetArguments(args.Skip(1)));
            }
            else
            {
                Console.Write(Help());
                return false;
            }
        }

        private async Task<bool> UpdateEntityActionAsync(IEnumerable<string> args)
        {
            var ar = args as string[] ?? args.ToArray();
            if (ar.Length != 1)
            {
                Console.WriteLine($"Command syntax: {CliApp.CommandName} update entity <entityName>");
                return false;
            }
            var name = ar.First();
            await UpdateEntityAsync(name);
            Console.WriteLine($"Entity {name} updated successfully");
            return true;
        }

        private async Task UpdateEntityAsync(string name)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var entities = await accessor.GetEntitiesAsync();
            var enums = await accessor.GetEnumsAsync();
            var newEntity = entities.FirstOrDefault(f => f.Name == name);
            if (newEntity == null)
            {
                throw new Exception($"Entity {name} not found");
            }

            var oldEntity = await GetOldEntityAsync(accessor, name, entities, enums);

            var generator = ServiceProvider.GetService<EntityGenerator>()!;
            await generator.UpgradeEntityAsync(accessor, oldEntity, newEntity);
            await generator.UpgradeEntityViewAsync(accessor, oldEntity, newEntity);
        }

        private async Task<Entity> GetOldEntityAsync(SolutionAccessor accessor, string name,
            List<Entity> currentEntities, List<Generator.Models.Enum> currentEnums)
        {
            var coreProject = accessor.Projects.First(p => p.Namespace.EndsWith(".Core"));
            var filePath = Path.Combine(coreProject.Path, "Metadata", "Entities", name + ".json");
            var gitRelativePath = Path.GetRelativePath(SolutionDirectory, filePath).Replace('\\', '/');

            var oldContent = await RunGitShowAsync($"HEAD:{gitRelativePath}");
            var oldEntity = oldContent.DeserializeFromJson<Entity>(new EntityTypeHandler());
            ResolveEntityRelationships(oldEntity, currentEntities, currentEnums);
            return oldEntity;
        }

        private static void ResolveEntityRelationships(Entity entity, List<Entity> entities, List<Generator.Models.Enum> enums)
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

        private async Task<bool> UpdateEnumActionAsync(IEnumerable<string> args)
        {
            var ar = args as string[] ?? args.ToArray();
            if (ar.Length != 1)
            {
                Console.WriteLine($"Command syntax: {CliApp.CommandName} update enum <enumName>");
                return false;
            }
            var name = ar.First();
            await UpdateEnumAsync(name);
            Console.WriteLine($"Enum {name} updated successfully");
            return true;
        }

        private async Task UpdateEnumAsync(string name)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var enums = await accessor.GetEnumsAsync();
            var newEnum = enums.FirstOrDefault(f => f.Name == name);
            if (newEnum == null)
            {
                throw new Exception($"Enum {name} not found");
            }

            var oldEnum = await GetOldEnumAsync(accessor, name);

            var generator = ServiceProvider.GetService<EnumGenerator>()!;
            await generator.UpgradeEnumAsync(accessor, oldEnum, newEnum);
            await generator.UpgradeEnumViewAsync(accessor, oldEnum, newEnum);
        }

        private async Task<Generator.Models.Enum> GetOldEnumAsync(SolutionAccessor accessor, string name)
        {
            var coreProject = accessor.Projects.First(p => p.Namespace.EndsWith(".Core"));
            var filePath = Path.Combine(coreProject.Path, "Metadata", "Enums", name + ".json");
            var gitRelativePath = Path.GetRelativePath(SolutionDirectory, filePath).Replace('\\', '/');

            var oldContent = await RunGitShowAsync($"HEAD:{gitRelativePath}");
            var oldEnum = oldContent.DeserializeFromJson<Generator.Models.Enum>();
            return oldEnum;
        }

        private async Task<string> RunGitShowAsync(string fileSpec)
        {
            var psi = new ProcessStartInfo
            {
                WorkingDirectory = SolutionDirectory,
                FileName = "git",
                Arguments = $"show {fileSpec}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception($"git show failed: {error}");
            }

            return output;
        }

        public override string Help()
        {
            return $"{CliApp.CommandName} update <action>\r\n" +
                   $"Available actions: \r\n{string.Join("\r\n", _actions.Keys.Select(s => $"\t{s}"))}\r\n";
        }
    }
}
