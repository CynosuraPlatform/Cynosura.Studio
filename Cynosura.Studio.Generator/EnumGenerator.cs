using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class EnumGenerator
    {
        private readonly CodeGenerator _codeGenerator;

        public EnumGenerator(CodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        public async Task GenerateEnumAsync(SolutionAccessor solution, Models.Enum @enum)
        {
            var model = new EnumModel(@enum, solution);
            await _codeGenerator.GenerateAsync(solution, model.GetGenerateInfo());
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

            await _codeGenerator.UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        public async Task GenerateEnumViewAsync(SolutionAccessor solution, Models.Enum @enum)
        {
            var views = await solution.GetViewsAsync();
            foreach (var view in views)
            {
                await GenerateEnumViewAsync(solution, @enum, view);
            }
        }

        public async Task GenerateEnumViewAsync(SolutionAccessor solution, Models.Enum @enum, View view)
        {
            var model = new EnumViewModel(view, @enum, solution);
            await _codeGenerator.GenerateAsync(solution, model.GetGenerateInfo());
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

            await _codeGenerator.UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
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
                await _codeGenerator.UpgradeAsync(toSolution, oldGenerateInfos, newGenerateInfos);
            }
        }
    }
}
