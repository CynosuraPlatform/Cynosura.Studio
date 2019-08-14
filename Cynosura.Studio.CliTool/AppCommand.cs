using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace Cynosura.Studio.CliTool
{
    public abstract class AppCommand
    {
        protected readonly string SolutionDirectory;
        protected readonly string Feed;
        protected readonly string Src;
        protected readonly string TemplateName;
        protected readonly ILifetimeScope LifetimeScope;

        protected AppCommand(string solutionDirectory, string feed, string src, string templateName, ILifetimeScope lifetimeScope)
        {
            SolutionDirectory = solutionDirectory;
            Feed = feed;
            Src = src;
            TemplateName = templateName;
            LifetimeScope = lifetimeScope;
        }

        protected IEnumerable<string> GetArguments(IEnumerable<string> args)
        {
            var ar = args.ToArray();
            var i = 0;
            while (ar.Length > i)
            {
                var part = ar[i];
                if (part.StartsWith("--"))
                {
                    i += 2;
                }

                yield return part;
                i++;
            }
        }

        public abstract Task<bool> ExecuteAsync(string[] args);

        public abstract string Help();
    }
}
