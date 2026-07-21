using blazor_boilerplate.Data;
using Microsoft.EntityFrameworkCore;
using blazor_boilerplate.Console.CommandRunner;

namespace blazor_boilerplate.Console.Commands;

public sealed class DatabaseMigrateCommand(ApplicationDbContext dbContext) : IConsoleCommand
{
    public string Name => "db:migrate";

    public string Description => "Applies pending Entity Framework Core migrations.";

    public async Task<int> ExecuteAsync(IReadOnlyList<string> args, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Applying database migrations...");

        await dbContext.Database.MigrateAsync(cancellationToken);

        System.Console.WriteLine("Database migrations are up to date.");
        return 0;
    }
}
