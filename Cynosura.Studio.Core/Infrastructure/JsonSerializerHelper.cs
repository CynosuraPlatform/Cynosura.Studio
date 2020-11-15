using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cynosura.Studio.Core.Infrastructure
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions JsonSerializerOptions
        {
            get
            {
                return new JsonSerializerOptions()
                {
                    Converters = { new TimeSpanConverter() }
                };
            }
        }
    }
}
