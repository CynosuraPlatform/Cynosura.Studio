using Cynosura.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly string _connectionString;
        private readonly ILoggerFactory _loggerFactory;
        private DataContext _dataContext;

        public DatabaseFactory(string connectionString, ILoggerFactory loggerFactory)
        {
            _connectionString = connectionString;
            _loggerFactory = loggerFactory;
        }

        public DbContext Get()
        {
            if (_dataContext == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlite(_connectionString);
                optionsBuilder.UseLoggerFactory(_loggerFactory);
                _dataContext = new DataContext(optionsBuilder.Options);
            }
            return _dataContext;
        }


        public void Dispose()
        {
            _dataContext?.Dispose();
        }
    }
}
