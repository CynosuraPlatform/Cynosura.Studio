using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Configurations
{
    public class WorkerScheduleTaskConfiguration : IEntityTypeConfiguration<WorkerScheduleTask>
    {
        public void Configure(EntityTypeBuilder<WorkerScheduleTask> builder)
        {
            builder.ToTable("WorkerScheduleTasks");
        }
    }
}
