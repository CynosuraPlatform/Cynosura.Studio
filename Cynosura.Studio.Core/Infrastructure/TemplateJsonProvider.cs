using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Cynosura.Studio.Core.Infrastructure
{
    public class TemplateJsonProvider : ITemplateProvider
    {
        private readonly TemplateJsonProviderOptions _options;
        public TemplateJsonProvider(IOptions<TemplateJsonProviderOptions> options)
        {
            _options = options.Value;
        }
        public Task<IEnumerable<TemplateModel>> GetTemplatesAsync()
        {
            return Task.FromResult(_options.AsEnumerable());
        }

        public Task<TemplateModel?> GetTemplateAsync(string name)
        {
            return Task.FromResult(_options.FirstOrDefault(f => f.Name == name));
        }
    }
}
