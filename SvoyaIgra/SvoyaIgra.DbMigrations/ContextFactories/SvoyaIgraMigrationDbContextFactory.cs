using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SvoyaIgra.DbMigrations.DbContexts;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SvoyaIgra.Shared.Constants;

namespace SvoyaIgra.DbMigrations.ContextFactories
{
    public class SvoyaIgraMigrationDbContextFactory : IDesignTimeDbContextFactory<SvoyaIgraDbMigrationContext>
    {
        public SvoyaIgraDbMigrationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SvoyaIgraDbMigrationContext>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            var config = builder.Build();
            var connectionString = config.GetConnectionString("SvoyaIgraDbContext");
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.MigrationsHistoryTable("__EFMigrationsHistory", DbConstants.SchemaSvoyaIgra);
            });

            return new SvoyaIgraDbMigrationContext(optionsBuilder.Options);
        }
    }
}
