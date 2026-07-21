using AppServices = blazor_boilerplate.Shared.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Radzen;

namespace blazor_boilerplate.Tests;

public class SharedUiServiceCollectionExtensionsTests
{
    [Fact]
    public void ThemeDefaultsUseMaterialLightAndDarkPair()
    {
        Assert.Equal("blazor-boilerplate-theme", AppServices.ThemeCookieName);
        Assert.Equal("material", AppServices.DefaultTheme);
        Assert.Equal("material-dark", AppServices.DefaultDarkTheme);
    }

    [Fact]
    public void AddSharedUiRegistersRadzenThemeServices()
    {
        var services = new ServiceCollection();

        AppServices.AddSharedUi(services);

        Assert.Contains(services, service => service.ServiceType == typeof(ThemeService));
        Assert.Contains(services, service => service.ServiceType == typeof(CookieThemeService));
    }

    [Fact]
    public void AddSharedUiConfiguresThemeCookiePersistence()
    {
        var services = new ServiceCollection();
        AppServices.AddSharedUi(services);

        using var provider = services.BuildServiceProvider();
        var options = provider.GetRequiredService<IOptions<CookieThemeServiceOptions>>().Value;

        Assert.Equal(AppServices.ThemeCookieName, options.Name);
        Assert.Equal(TimeSpan.FromDays(365), options.Duration);
    }
}
