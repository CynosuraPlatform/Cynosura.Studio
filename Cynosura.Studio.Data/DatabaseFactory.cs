using Autofac;
using Cynosura.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly ILifetimeScope _lifetimeScope;
        private DataContext _dataContext;

        public DatabaseFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public DbContext Get()
        {
            if (_dataContext == null)
            {
                _dataContext = _lifetimeScope.Resolve<DataContext>();
            }
            return _dataContext;
        }


        public void Dispose()
        {
            _dataContext?.Dispose();
        }
    }
}
