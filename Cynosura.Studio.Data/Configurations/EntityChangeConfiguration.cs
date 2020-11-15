using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Configurations
{
    public class EntityChangeConfiguration : IEntityTypeConfiguration<EntityChange>
    {
        public void Configure(EntityTypeBuilder<EntityChange> builder)
        {
            builder.ToTable("EntityChanges");
        }
    }
}
