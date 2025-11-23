using EngageGov.Domain.Interfaces;
using EngageGov.Infrastructure.Data;
using EngageGov.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EngageGov.Infrastructure;

/// <summary>
/// Dependency injection configuration for Infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Add DbContext with SQL Server (can be switched to in-memory for testing)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Use in-memory database if no connection string is provided
            services.AddDbContext<EngageGovDbContext>(options =>
                options.UseInMemoryDatabase("EngageGovDb"));
        }
        else
        {
            services.AddDbContext<EngageGovDbContext>(options =>
                options.UseNpgsql(connectionString, 
                    b => b.MigrationsAssembly(typeof(EngageGovDbContext).Assembly.FullName)));
        }

        // Register repositories
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<ICitizenRepository, CitizenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
