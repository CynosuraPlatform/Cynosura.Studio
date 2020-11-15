using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Configurations
{
    public class FileGroupConfiguration : IEntityTypeConfiguration<FileGroup>
    {
        public void Configure(EntityTypeBuilder<FileGroup> builder)
        {
            builder.ToTable("FileGroups");
        }
    }
}
