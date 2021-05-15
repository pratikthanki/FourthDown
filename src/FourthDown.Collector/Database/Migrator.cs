using System.Reflection;
using DbUp;
using FourthDown.Collector.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FourthDown.Collector.Database
{
    public interface IMigrator
    {
        MigrationResult UpgradeDatabase();
    }

    public class Migrator : IMigrator
    {
        private readonly ILogger _logger;
        private readonly DatabaseOptions _databaseOptions;

        public Migrator(
            ILogger<Migrator> logger,
            IOptions<DatabaseOptions> databaseOptions)
        {
            _logger = logger;
            _databaseOptions = databaseOptions.Value;
        }

        public MigrationResult UpgradeDatabase()
        {
            EnsureDatabase.For.SqlDatabase(_databaseOptions.ConnectionString);

            var upgradeEngine = DeployChanges.To
                .SqlDatabase(_databaseOptions.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .JournalToSqlTable("dbo", "SchemaVersions")
                .LogScriptOutput()
                .Build();

            if (!upgradeEngine.IsUpgradeRequired())
            {
                _logger.LogError("Database upgrade is not required");
                return MigrationResult.NotRequired;
            }

            var upgradeResult = upgradeEngine.PerformUpgrade();
            if (upgradeResult.Successful)
            {
                return MigrationResult.Success;
            }

            _logger.LogCritical(upgradeResult.Error.Message);

            return MigrationResult.Failure;
        }
    }

    public enum MigrationResult
    {
        Failure = 0,
        NotRequired = 1,
        Success = 2
    }
}