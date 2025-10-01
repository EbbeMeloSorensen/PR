using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace PR.Web.Application.Core;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(assemblies: typeof(Application.Smurfs.List.Handler).Assembly);
        services.AddMediatR(assemblies: typeof(Application.People.List.Handler).Assembly);
        
        return services;
    }
}    
