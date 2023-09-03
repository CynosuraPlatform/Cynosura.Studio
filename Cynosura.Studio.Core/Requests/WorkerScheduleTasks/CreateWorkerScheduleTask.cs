﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.WorkerScheduleTasks
{
    public class CreateWorkerScheduleTask : IRequest<CreatedEntity<int>>
    {
        public int? WorkerInfoId { get; set; }

        public string? Seconds { get; set; }

        public string? Minutes { get; set; }

        public string? Hours { get; set; }

        public string? DayOfMonth { get; set; }

        public string? Month { get; set; }

        public string? DayOfWeek { get; set; }

        public string? Year { get; set; }
    }
}
