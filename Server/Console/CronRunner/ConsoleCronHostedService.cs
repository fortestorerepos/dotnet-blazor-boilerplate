using blazor_boilerplate.Console.CommandRunner;

namespace blazor_boilerplate.Console.Crons;

public sealed class ConsoleCronHostedService(
    IEnumerable<ConsoleCronRegistration> registrations,
    IServiceScopeFactory scopeFactory,
    ILogger<ConsoleCronHostedService> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cronTasks = registrations.Select(registration => RunCronAsync(registration, stoppingToken)).ToArray();

        return cronTasks.Length == 0
            ? Task.CompletedTask
            : Task.WhenAll(cronTasks);
    }

    private async Task RunCronAsync(ConsoleCronRegistration registration, CancellationToken stoppingToken)
    {
        if (registration.RunOnStartup)
        {
            await RunOnceAsync(registration, stoppingToken);
        }

        using var timer = new PeriodicTimer(registration.Interval);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunOnceAsync(registration, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }

    private async Task RunOnceAsync(ConsoleCronRegistration registration, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var command = scope.ServiceProvider
                .GetServices<IConsoleCommand>()
                .FirstOrDefault(command =>
                    string.Equals(command.Name, registration.CommandName, StringComparison.OrdinalIgnoreCase));

            if (command is null)
            {
                logger.LogError("Cron command {CommandName} is not registered", registration.CommandName);
                return;
            }

            logger.LogInformation("Running cron command {CommandName}", registration.CommandName);
            var exitCode = await command.ExecuteAsync(registration.Arguments, stoppingToken);
            logger.LogInformation("Finished cron command {CommandName} with exit code {ExitCode}", registration.CommandName, exitCode);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Cron command {CommandName} failed", registration.CommandName);
        }
    }
}
