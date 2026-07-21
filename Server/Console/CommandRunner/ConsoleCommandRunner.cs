namespace blazor_boilerplate.Console.CommandRunner;

public sealed class ConsoleCommandRunner(IServiceScopeFactory scopeFactory)
{
    private const string EntryCommand = "console";

    public static bool TryGetConsoleArgs(string[] args, out string[] consoleArgs)
    {
        if (args.Length > 0 && string.Equals(args[0], EntryCommand, StringComparison.OrdinalIgnoreCase))
        {
            consoleArgs = args[1..];
            return true;
        }

        consoleArgs = [];
        return false;
    }

    public async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var commands = scope.ServiceProvider
            .GetServices<IConsoleCommand>()
            .OrderBy(command => command.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (args.Length == 0 || IsHelp(args[0]))
        {
            PrintHelp(commands, args.Skip(1).FirstOrDefault());
            return 0;
        }

        if (string.Equals(args[0], "list", StringComparison.OrdinalIgnoreCase))
        {
            PrintCommandList(commands);
            return 0;
        }

        var command = commands.FirstOrDefault(command =>
            string.Equals(command.Name, args[0], StringComparison.OrdinalIgnoreCase));

        if (command is null)
        {
            System.Console.Error.WriteLine($"Unknown command: {args[0]}");
            System.Console.Error.WriteLine();
            PrintCommandList(commands);
            return 1;
        }

        return await command.ExecuteAsync(args[1..], cancellationToken);
    }

    private static bool IsHelp(string value)
    {
        return string.Equals(value, "help", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "-h", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "--help", StringComparison.OrdinalIgnoreCase);
    }

    private static void PrintHelp(IReadOnlyCollection<IConsoleCommand> commands, string? commandName)
    {
        if (!string.IsNullOrWhiteSpace(commandName))
        {
            var command = commands.FirstOrDefault(command =>
                string.Equals(command.Name, commandName, StringComparison.OrdinalIgnoreCase));

            if (command is null)
            {
                System.Console.Error.WriteLine($"Unknown command: {commandName}");
                return;
            }

            System.Console.WriteLine(command.Name);
            System.Console.WriteLine(command.Description);
            System.Console.WriteLine();
            System.Console.WriteLine($"Usage: dotnet run --project Server -- console {command.Usage}");
            return;
        }

        System.Console.WriteLine("Usage: dotnet run --project Server -- console <command> [arguments]");
        System.Console.WriteLine();
        PrintCommandList(commands);
    }

    private static void PrintCommandList(IReadOnlyCollection<IConsoleCommand> commands)
    {
        System.Console.WriteLine("Available commands:");

        foreach (var command in commands)
        {
            System.Console.WriteLine($"  {command.Name.PadRight(18)} {command.Description}");
        }

        System.Console.WriteLine();
        System.Console.WriteLine("Run `dotnet run --project Server -- console help <command>` for details.");
    }
}
