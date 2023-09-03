﻿using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.WorkerRuns.Models
{
    public class WorkerRunFilter : EntityFilter
    {
        public int? WorkerInfoId { get; set; }
        public Core.Enums.WorkerRunStatus? Status { get; set; }
        public DateTime? StartDateTimeFrom { get; set; }
        public DateTime? StartDateTimeTo { get; set; }
        public DateTime? EndDateTimeFrom { get; set; }
        public DateTime? EndDateTimeTo { get; set; }
        public string? Data { get; set; }
        public string? Result { get; set; }
        public string? ResultData { get; set; }
    }
}
