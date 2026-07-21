using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace blazor_boilerplate.Shared;

public static class ServiceCollectionExtensions
{
    public const string ThemeCookieName = "blazor-boilerplate-theme";
    public const string DefaultTheme = "material";
    public const string DefaultDarkTheme = "material-dark";

    public static IServiceCollection AddSharedUi(this IServiceCollection services)
    {
        services.AddRadzenComponents();
        services.AddRadzenCookieThemeService(options =>
        {
            options.Name = ThemeCookieName;
            options.Duration = TimeSpan.FromDays(365);
        });

        return services;
    }
}
