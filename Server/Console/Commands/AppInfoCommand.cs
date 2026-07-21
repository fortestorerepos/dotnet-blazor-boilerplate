using Microsoft.Extensions.Hosting;
using blazor_boilerplate.Console.CommandRunner;

namespace blazor_boilerplate.Console.Commands;

public sealed class AppInfoCommand(
    IConfiguration configuration,
    IHostEnvironment environment) : IConsoleCommand
{
    public string Name => "app:info";

    public string Description => "Shows basic application configuration details.";

    public Task<int> ExecuteAsync(IReadOnlyList<string> args, CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"Application: {environment.ApplicationName}");
        System.Console.WriteLine($"Environment: {environment.EnvironmentName}");
        System.Console.WriteLine($"Content root: {environment.ContentRootPath}");
        System.Console.WriteLine($"Default connection configured: {configuration.GetConnectionString("DefaultConnection") is not null}");

        return Task.FromResult(0);
    }
}
