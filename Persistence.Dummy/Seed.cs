using PR.Persistence.EntityFrameworkCore;

namespace Persistence.Dummy
{
    public class Seed
    {
        public static async Task SeedData(
            DataContext2 context)
        {
            if (!context.Smurfs.Any())
            {
                Seeding.CreateDataForSeeding(
                    true, 
                    out var people, 
                    out var personComments,
                    out var smurfs);

                // await context.People.AddRangeAsync(people);
                // await context.PersonComments.AddRangeAsync(personComments);
                await context.Smurfs.AddRangeAsync(smurfs);
                await context.SaveChangesAsync();
            }
        }
    }
}
