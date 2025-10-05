﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Persistence.Dummy;
using PR.Persistence;
using PR.Persistence.EntityFrameworkCore;
using PR.Web.Application.Core;
using PR.Web.Application.Interfaces;
using PR.Web.Application.People;
using PR.Web.Application.Smurfs;
using PR.Web.Infrastructure.Pagination;
using PR.Web.Infrastructure.Security;
using PR.Web.Persistence;

namespace PR.Web.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config,
        bool deployingToHeroku)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
        });

        var connectionString = string.Empty;

        if (deployingToHeroku)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Depending on if in development or production, use either Heroku-provided
            // connection string, or development connection string from env var.
            if (env == "Development")
            {
                // Use connection string from file.
                connectionString = config.GetConnectionString("DefaultConnection");
            }
            else
            {
                // Use connection string provided at runtime by Heroku.
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                // Parse connection URL to connection string for Npgsql
                connUrl = connUrl.Replace("postgres://", string.Empty);
                var pgUserPass = connUrl.Split("@")[0];
                var pgHostPortDb = connUrl.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];
                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];

                connectionString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
            }
        }
        else
        {
            connectionString = config.GetConnectionString("DefaultConnection");
        }

        services.AddIdentityPersistence<DataContext>(options => 
        {
            //options.UseSqlite(connectionString);
            options.UseNpgsql(connectionString);
            //options.UseSqlServer(connectionString);
        });

        services.AddDummyPersistence<DataContext2>(options => 
        {
            //options.UseSqlite(connectionString);
            options.UseNpgsql(connectionString);
            //options.UseSqlServer(connectionString);
        });

        services.AddAppDataPersistence<PRDbContextBase>(options =>
        {
            //options.UseSqlite(connectionString);
            options.UseNpgsql(connectionString);
            //options.UseSqlServer(connectionString);
        });




        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000");
            });
        });
        // services.AddMediatR(assemblies: typeof(Application.Smurfs.List.Handler).Assembly);
        // services.AddMediatR(assemblies: typeof(Application.People.List.Handler).Assembly);
        services.AddApplication();
        services.AddAutoMapper(assemblies: typeof(MappingProfiles).Assembly);
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.AddScoped<IPagingHandler<SmurfDto>, PagingHandler<SmurfDto>>();
        services.AddScoped<IPagingHandler<PersonDto>, PagingHandler<PersonDto>>();

        return services;
    }
}