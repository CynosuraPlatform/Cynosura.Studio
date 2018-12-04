using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Core.Infrastructure
{
    public class StudioException: Exception
    {
        public string Code { get; set; }

        public StudioException(string code) : base()
        {
            Code = code;
        }

        public StudioException(string message, string code) : base(message)
        {
            Code = code;
        }

        public StudioException(string message, string code, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}
