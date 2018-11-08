using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Data
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {
        public event EventHandler SavingChanges;

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            
        }

        public override int SaveChanges()
        {
            OnSavingChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnSavingChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyAllConfigurations();
        }

        protected virtual void OnSavingChanges()
        {
            SavingChanges?.Invoke(this, EventArgs.Empty);
        }
    }
}
