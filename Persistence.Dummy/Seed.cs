using PR.Domain.Entities.Smurfs;

namespace Persistence.Dummy
{
    public class Seed
    {
        public static async Task SeedData(
            DataContext2 context)
        {
            if (!context.Smurfs.Any())
            {
                var smurfs = new List<Smurf>();
                smurfs.Add(new Smurf { Name = "Gammelsmølf" });
                smurfs.Add(new Smurf { Name = "Smølfine" });
                smurfs.Add(new Smurf { Name = "Skæmtesmølf" });
                smurfs.Add(new Smurf { Name = "Dydigsmølfen" });
                smurfs.Add(new Smurf { Name = "Gnavensmølfen" });
                smurfs.Add(new Smurf { Name = "Flyvesmølfen" });
                smurfs.Add(new Smurf { Name = "Pyntesmølfen" });
                smurfs.Add(new Smurf { Name = "Dummesmølfen" });
                smurfs.Add(new Smurf { Name = "Dovensmølfen" });
                smurfs.Add(new Smurf { Name = "Stærksmølfen" });
                smurfs.Add(new Smurf { Name = "Ædesmølfen" });

                Enumerable.Range(1, 100).ToList().ForEach(i =>
                {
                    smurfs.Add(new Smurf { Name = $"Ekstra smølf {i}" });
                });

                await context.Smurfs.AddRangeAsync(smurfs);
                await context.SaveChangesAsync();
            }
        }
    }
}
