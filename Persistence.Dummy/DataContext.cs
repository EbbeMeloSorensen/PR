using Microsoft.EntityFrameworkCore;

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

        public DbSet<Dummy> Dummies { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DummyConfiguration());
        }
    }
}

