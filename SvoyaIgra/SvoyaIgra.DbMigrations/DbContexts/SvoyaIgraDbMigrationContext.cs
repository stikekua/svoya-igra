using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvoyaIgra.DbMigrations.DbContexts
{
    public class SvoyaIgraDbMigrationContext: DbContext
    {
        public SvoyaIgraDbMigrationContext(DbContextOptions<SvoyaIgraDbMigrationContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddSvoyaIgraModel();

            Data.InitialData.CreateInitialData(modelBuilder);
        }
    }
}
