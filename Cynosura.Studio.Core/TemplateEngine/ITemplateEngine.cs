using System.Globalization;

namespace Cynosura.Studio.Core.TemplateEngine
{
    public interface ITemplateEngine
    {
        string ProcessTemplate(string templateFile, object model, CultureInfo cultureInfo = null);
    }
}
