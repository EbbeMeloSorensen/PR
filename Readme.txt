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
   af ASP.NET Core .. Du er nødt til at lade det være båret af nogle tegninger af, hvad 
   der afhænger af hvad, hvor det f.eks. fremgår, at APIen afhænger af:
   - de forskellige contexts - bruges til migrering, dvs til at lave databasen under opstart
   - En IUserAccessor (abstraktion) - den bruger den i øvrigt ikke mere - den blev brugt i Reactivities, da forskellige brugere jo kunne vedligeholde forskellige ting, men det lavede du jo om, så man bare ser alt og kan ændre alt, hvis bare man har logget ind
   - En IUnitOfWorkFactory (abstraktion) - bruges til at vedligeholde
   - APIens AccountController afhænger af en UserManager<AppUser>, en SignInManager<AppUser> og 
     en TokenService, som har en metode, der tager en AppUser.

I hvilken henseender er det lige at APIen afhænger af application laget?
- IUserAccessor er defineret i application laget   
- De controllers, der defineres i APIen sender videre til Queries, Commands og Handlers i 
  application laget

Mermaid fra ChatGPT, som illustrerer konceptet:
flowchart TD

    subgraph Presentation["Presentation Layer"]
        WPFApp["WPF Application"]
        CLI["Console Application"]
        API["Web API (ASP.NET Core)"]
    end

    subgraph Application["Application Layer"]
        AppServices["Application Services (Use Cases)"]
        IRepository["IRepository Interfaces"]
        IUserService["IUserService Interface"]
    end

    subgraph Persistence["Persistence Layer"]
        EFRepository["EF Repository (AppDataContext)"]
        AppDataContext["AppDataContext (DbContext)"]
    end

    subgraph Identity["Identity Infrastructure"]
        IdentityService["IdentityService (IUserService impl)"]
        IdentityDataContext["IdentityDataContext (IdentityDbContext)"]
    end

    %% Connections
    WPFApp --> AppServices
    CLI --> AppServices
    API --> AppServices

    AppServices --> IRepository
    AppServices --> IUserService

    IRepository <--> EFRepository
    EFRepository --> AppDataContext

    IUserService <--> IdentityService
    IdentityService --> IdentityDataContext



Et andet mermaid diagram, som illustrerer, at det er legitimt for APIet at afhænge af
ASP.NET Core Identity - som den siger: "Those concerns are NOT BUSINESS LOGIC - THEY BELONG AT THE API boundary"
..og det er jo altså Application lagets opgave at tage sig af business logik

flowchart TD

    subgraph Presentation["Presentation Layer"]
        API["Web API (ASP.NET Core)"]
    end

    subgraph Application["Application Layer"]
        AppServices["Application Services (Use Cases)"]
        IRepository["IRepository<T>"]
        IUserService["IUserService"]
    end

    subgraph Persistence["Persistence Layer"]
        EfRepository["EF Repository"]
        AppDataContext["AppDataContext (DbContext)"]
    end

    subgraph Identity["Identity Infrastructure"]
        IdentityUserService["IdentityUserService (IUserService impl)"]
        IdentityDataContext["IdentityDataContext (IdentityDbContext)"]
        IdentityFramework["ASP.NET Core Identity (UserManager, SignInManager)"]
    end

    %% Business flow
    API -->|Business requests 🟢| AppServices
    AppServices --> IRepository
    AppServices --> IUserService

    IRepository <..> EfRepository
    EfRepository --> AppDataContext

    IUserService <..> IdentityUserService
    IdentityUserService --> IdentityDataContext
    IdentityUserService --> IdentityFramework

    %% Auth-only flow
    API -->|Auth ops 🔴| IdentityFramework




Opdatering 29-09-2025:
  Jeg havde et gennembrud fredag den 26. september, hvor det lykkedes mig at fjerne afhængighederne
  fra Application laget til ASP.Net Core og endda Entity Framework Core ved at introducere en
  abstraktion for paging fukntionaliteten (IPagingHandler). Det virker med login og at det at hente
  smurfs, men jeg har også fået wrecket det at hente personer - det er ikke uventet, idet
  de 2 migrations, som er i spil, nu ikke længere trækker på PR.Persistence.EntityFramework i
  forbindelse med at generere tabeller i databasen. Jeg prøvede så at refaktorere det således at
  APIen trak på den DbContext, som bor i PR.Persistence.EntityFrameworkCore for at migrere person-
  relaterede tabeller. Det lykkedes ikke, og en af årsagerne er, at den ifølge ChatGPT er lavet på
  en lidt gammeldags måde, f.eks derved at den ikke har en constructor, der tager en DbContextOptions
  som parameter. Bemærk i øvrigt, at det skulle kunne lade sig gøre at slippe for de der 
  dbms-specifikke plugins med deres ConnectionStringProviders, da essensen af den nye metode er, 
  at dbcontexts struktureres "udefra". ChatGPT kender til principperne.

  Spørgsmålet er nu, hvordan faen jeg bevæger mig videre.. Jeg har nok i hvert fald gavn af at
  tage de der C2IEDM-ting ud, og muligvis bør jeg også tage personer ud. Et andet spor kunne være
  at lave Persistence.Dummy ud, så den ikke dealer i smurfs, men måske dummies...