using Microsoft.EntityFrameworkCore;
//using PR.Persistence.EntityFrameworkCore;
//using PR.Domain.Entities.PR;
using PR.Domain.Entities.Smurfs;
//using PR.Domain.Entities.C2IEDM.ObjectItems;

namespace Persistence.Dummy
{
    public class DataContext : DbContext
    {
        public DataContext(
            DbContextOptions options) : base(options)
        {
        }

        public DbSet<Smurf> Smurfs { get; set; }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            // PRDbContextBase.Versioned = true;
            // PRDbContextBase.Configure(builder);

            base.OnModelCreating(builder);
        }
    }
}

