using EngageGov.Application.Interfaces;
using EngageGov.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EngageGov.Application;

/// <summary>
/// Dependency injection configuration for Application layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IProposalService, ProposalService>();
        services.AddScoped<ICitizenService, CitizenService>();
    // External gov service is registered in the API project where HttpClientFactory is available

        return services;
    }
}
