using System;
using System.ComponentModel;

namespace Cynosura.Studio.Core.Enums
{
    public enum WorkerRunStatus
    {
        [Description("New")]
        New = 0,
        [Description("Running")]
        Running = 1,
        [Description("Completed")]
        Completed = 2,
        [Description("Error")]
        Error = 3
    }
}
