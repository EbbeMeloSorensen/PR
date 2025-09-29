using Microsoft.EntityFrameworkCore;
//using PR.Persistence.EntityFrameworkCore;
//using PR.Domain.Entities.PR;
using PR.Domain.Entities.Smurfs;
//using PR.Domain.Entities.C2IEDM.ObjectItems;

// NB For at lave migration for denne DataContext skal man eksekvere dette:
// dotnet ef migrations add InitialMigration -p Persistence.Dummy -s PR.Web.API --context DataContext2
// .. Det virkede i hvert fald på Linux laptoppen, hvor jeg kunne spinne denne version af APIen op
// og både logge ind og hente Smurfs. 

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

