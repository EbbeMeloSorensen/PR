API-projektet afhænger nu af 2 forskellige dbcontexts. En, som har med authentication at gøre,
og en, som har med domæneobjekter at gøre. De har også hver deres migration, men derfor
targetter de alligevel samme database, og det lader til at virke fint.

De controllere, der dealer i domæneobjekter, bruger bare UnitOfWorkFactoryen, som er et 
abstraktionslag over DbContexten.

Næste trin må vel være noget alla følgende:
1) Introducer en afhængighed fra API'et til en TREDJE dbcontext - specifikt den,
   der allerede eksisterer og dealer i personer osv
2) Fjern den der dummy-persistence ting, så du igen er nede på to db contexts
3) Rens op i afhængighederne, som anvist af ChatGpt, så applikationslaget ikke afhænger
   af ASP.NET Core