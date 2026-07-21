using blazor_boilerplate.Console.Commands;
using blazor_boilerplate.Console.Crons;
using blazor_boilerplate.Console.CommandRunner;

namespace blazor_boilerplate.Console;

public static class ConsoleServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleHost(this IServiceCollection services)
    {
        services.AddSingleton<ConsoleCommandRunner>();
        services.AddScoped<IConsoleCommand, AppHeartbeatCommand>();
        services.AddScoped<IConsoleCommand, AppInfoCommand>();
        services.AddScoped<IConsoleCommand, DatabaseMigrateCommand>();

        services.AddConsoleCron(
            commandName: "app:heartbeat",
            interval: TimeSpan.FromMinutes(15),
            runOnStartup: true);

        services.AddHostedService<ConsoleCronHostedService>();

        return services;
    }

    public static IServiceCollection AddConsoleCron(
        this IServiceCollection services,
        string commandName,
        TimeSpan interval,
        bool runOnStartup = false,
        params string[] arguments)
    {
        if (string.IsNullOrWhiteSpace(commandName))
        {
            throw new ArgumentException("Cron command name cannot be empty.", nameof(commandName));
        }

        if (interval <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(interval), interval, "Cron interval must be greater than zero.");
        }

        services.AddSingleton(new ConsoleCronRegistration(commandName, arguments, interval, runOnStartup));

        return services;
    }

    public static async Task<int?> RunConsoleCommandIfRequestedAsync(this WebApplication app, string[] args)
    {
        if (!ConsoleCommandRunner.TryGetConsoleArgs(args, out var consoleArgs))
        {
            return null;
        }

        return await app.Services.GetRequiredService<ConsoleCommandRunner>()
            .RunAsync(consoleArgs, app.Lifetime.ApplicationStopping);
    }
}
