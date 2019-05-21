using System.Globalization;

namespace Cynosura.Studio.Generator.TemplateEngine
{
    public interface ITemplateEngine
    {
        string ProcessTemplate(string templateFile, object model, CultureInfo cultureInfo = null);
    }
}
