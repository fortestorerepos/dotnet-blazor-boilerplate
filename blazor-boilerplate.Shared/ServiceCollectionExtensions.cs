using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace blazor_boilerplate.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedUi(this IServiceCollection services)
    {
        services.AddRadzenComponents();

        return services;
    }
}
