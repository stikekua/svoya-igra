using Microsoft.EntityFrameworkCore;
using SvoyaIgra.Dal;

namespace SvoyaIgra.TestClient.Data
{
    internal class SvoyaIgraDbContext : DbContext
    {
        public SvoyaIgraDbContext(DbContextOptions<SvoyaIgraDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddQBankModel();
            modelBuilder.AddGameModel();
        }
    }
}
