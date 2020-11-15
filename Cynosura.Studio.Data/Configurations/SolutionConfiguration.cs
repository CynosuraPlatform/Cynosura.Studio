using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Configurations
{
    public class SolutionConfiguration : IEntityTypeConfiguration<Solution>
    {
        public void Configure(EntityTypeBuilder<Solution> builder)
        {
            builder.ToTable("Solutions");
        }
    }
}
