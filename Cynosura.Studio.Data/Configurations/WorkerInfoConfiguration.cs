using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Configurations
{
    public class WorkerInfoConfiguration : IEntityTypeConfiguration<WorkerInfo>
    {
        public void Configure(EntityTypeBuilder<WorkerInfo> builder)
        {
            builder.ToTable("WorkerInfos");
        }
    }
}
