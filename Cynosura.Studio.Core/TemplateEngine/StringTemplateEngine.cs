using System;
using System.Globalization;
using Antlr4.StringTemplate;

namespace Cynosura.Studio.Core.TemplateEngine
{
    public class StringTemplateEngine : ITemplateEngine
    {
        public string ProcessTemplate(string templateFile, object model, CultureInfo cultureInfo = null)
        {
            var stg = new TemplateGroupFile(templateFile, '$', '$');
            stg.RegisterRenderer(typeof(Decimal), new DecimalRenderer());
            stg.RegisterRenderer(typeof(DateTime), new DateTimeRenderer());
            var st = stg.GetInstanceOf("main");
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
    }
}
