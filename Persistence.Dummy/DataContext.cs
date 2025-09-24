using Microsoft.EntityFrameworkCore;
//using PR.Persistence.EntityFrameworkCore;
//using PR.Domain.Entities.PR;
using PR.Domain.Entities.Smurfs;
//using PR.Domain.Entities.C2IEDM.ObjectItems;

namespace Persistence.Dummy
{
    public class DataContext2 : DbContext
    {
        public DataContext2(
            DbContextOptions<DataContext2> options) : base(options)
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

