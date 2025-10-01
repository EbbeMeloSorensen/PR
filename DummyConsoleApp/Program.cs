using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DummyConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hej");

            // 1. Setup DI container
            var services = new ServiceCollection();

            //// register Application (your extension method)
            //services.AddApplication();

            //// register MediatR (scan Application assembly)
            //services.AddMediatR(cfg =>
            //    cfg.RegisterServicesFromAssemblyContaining<Create.Command>());

            //var provider = services.BuildServiceProvider();

            //// 2. Resolve IMediator
            //var mediator = provider.GetRequiredService<IMediator>();

            //// 3. Use commands/queries like the API does
            //Console.WriteLine("Creating a person...");
            //var result = await mediator.Send(new Create.Command
            //{
            //    FirstName = "Ada",
            //    LastName = "Lovelace"
            //});

            //Console.WriteLine(result.IsSuccess
            //    ? "✅ Person created"
            //    : $"❌ Failed: {result.Error}");

            //Console.WriteLine("Listing all people...");
            //var list = await mediator.Send(new List.Query { PageNumber = 1, PageSize = 10 });

            //foreach (var person in list.Value.Items)
            //{
            //    Console.WriteLine($"- {person.FirstName} {person.LastName}");
            //}
        }
    }
}


//namespace PR.Web.Application
//{
//    public static class ServiceCollectionExtensions
//    {
//        public static IServiceCollection AddApplication(
//            this IServiceCollection services,
//            string connectionString = null)
//        {
//            // Register MediatR
//            services.AddMediatR(cfg =>
//                cfg.RegisterServicesFromAssemblyContaining<ServiceCollectionExtensions>());

//            // Register Persistence dependencies
//            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

//            // If a connection string is provided, register EF Core
//            if (!string.IsNullOrEmpty(connectionString))
//            {
//                services.AddDbContextFactory<PRDbContextBase>(options =>
//                    options.UseSqlite(connectionString));
//            }

//            return services;
//        }
//    }
//}