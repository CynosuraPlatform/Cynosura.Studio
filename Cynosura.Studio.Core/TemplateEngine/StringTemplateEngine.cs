using System;
using System.Globalization;
using System.Text;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.TemplateEngine
{
    public class StringTemplateEngine : ITemplateEngine
    {
        public string ProcessTemplate(string templateFile, object model, CultureInfo cultureInfo = null)
        {
            var stg = new CustomTemplateGroupFile(templateFile);
            stg.RegisterRenderer(typeof(Decimal), new DecimalRenderer());
            stg.RegisterRenderer(typeof(DateTime), new DateTimeRenderer());
            stg.RegisterModelAdaptor(typeof(PropertyCollection), new PropertyCollectionAdapter());
            var st = stg.GetInstanceOf("main");
            if (st == null)
            {
                throw new StudioException($"{templateFile} template error","StringTemplateEngine/Parsing");
            }
            st.Add("model", model);
            return cultureInfo == null ? st.Render() : st.Render(cultureInfo);
        }

        class DecimalRenderer : IAttributeRenderer
        {
            public string ToString(object obj, string formatString, CultureInfo culture)
            {
                if (obj == null)
                    return "";
                var dec = (decimal)obj;
                if (!string.IsNullOrEmpty(formatString))
                    return dec.ToString(formatString, culture);
                else
                    return dec.ToString(culture);
            }
        }

        class DateTimeRenderer : IAttributeRenderer
        {
            public string ToString(object obj, string formatString, CultureInfo culture)
            {
                if (obj == null)
                    return "";
                var date = (DateTime)obj;
                if (string.IsNullOrEmpty(formatString))
                    formatString = "dd.MM.yyyy HH:mm:ss";
                return date.ToString(formatString, culture);
            }
        }

        class PropertyCollectionAdapter : IModelAdaptor
        {
            public object GetProperty(Interpreter interpreter, TemplateFrame frame, object obj, object property, string propertyName)
            {
                if (obj is PropertyCollection collection)
                {
                    return collection[propertyName];
                }

                return "";
            }
        }
    }
}
