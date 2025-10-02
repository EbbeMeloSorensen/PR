using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace PR.Web.Application.Core;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(People.List.Handler).Assembly));

        services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(Smurfs.List.Handler).Assembly));
        
        return services;
    }
}    
