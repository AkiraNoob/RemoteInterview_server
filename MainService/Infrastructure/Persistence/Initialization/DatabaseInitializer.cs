using MainService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MainService.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ApplicationDbSeeder _dbSeeder;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IOptions<DatabaseSettings> _options;

    public DatabaseInitializer(ApplicationDbContext dbContext, ILogger<DatabaseInitializer> logger, ApplicationDbSeeder dbSeeder, IOptions<DatabaseSettings> options)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbSeeder = dbSeeder;
        _options = options;
    }

    public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
    {
        await InitializeTenantDbAsync(cancellationToken);
    }

    private async Task InitializeTenantDbAsync(CancellationToken cancellationToken)
    {
        if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            _logger.LogInformation("Applying Root Migrations.");

            await _dbContext.Database.MigrateAsync(cancellationToken);
        }

        await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
    }
}