using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SvoyaIgra.DbMigrations.DbContexts;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.DbMigrations.ContextFactories
{
    public class GameMigrationDbContextFactory : IDesignTimeDbContextFactory<GameDbMigrationContext>
    {
        public GameDbMigrationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameDbMigrationContext>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            var config = builder.Build();
            var connectionString = config.GetConnectionString("SvoyaIgraDbContext");
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsHistoryTable("__EFMigrationsHistory", DbConstants.SchemaGame);
            });

            return new GameDbMigrationContext(optionsBuilder.Options);
        }
    }
}
