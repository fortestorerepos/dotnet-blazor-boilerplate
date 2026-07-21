namespace blazor_boilerplate.Console.CommandRunner;

public interface IConsoleCommand
{
    string Name { get; }

    string Description { get; }

    string Usage => Name;

    Task<int> ExecuteAsync(IReadOnlyList<string> args, CancellationToken cancellationToken);
}
