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
