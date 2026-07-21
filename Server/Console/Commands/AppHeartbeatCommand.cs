using blazor_boilerplate.Console.CommandRunner;

namespace blazor_boilerplate.Console.Commands;

public sealed class AppHeartbeatCommand(ILogger<AppHeartbeatCommand> logger) : IConsoleCommand
{
    public string Name => "app:heartbeat";

    public string Description => "Writes a heartbeat log entry.";

    public Task<int> ExecuteAsync(IReadOnlyList<string> args, CancellationToken cancellationToken)
    {
        logger.LogInformation("Heartbeat command ran at {Timestamp:O}", DateTimeOffset.UtcNow);

        return Task.FromResult(0);
    }
}
