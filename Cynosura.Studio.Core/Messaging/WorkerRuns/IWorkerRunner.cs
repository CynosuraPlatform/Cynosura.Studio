﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Messaging.WorkerRuns
{
    public interface IWorkerRunner
    {
        Task<int> RunAsync(Type workerType, object? data);
    }
}
