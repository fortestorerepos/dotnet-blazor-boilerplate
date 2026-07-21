# dotnet-blazor-boilerplate

## Console commands

Run app commands through the server project:

```bash
dotnet run --project Server -- console list
dotnet run --project Server -- console app:info
dotnet run --project Server -- console db:migrate
```

With `make`:

```bash
make console ARGS="list"
make console ARGS="app:info"
```

Add new commands in `Server/Console/Commands` by implementing `IConsoleCommand`, then register them in `AddConsoleHost`.

## Crons

Scheduled jobs run existing console commands. Add the work as an `IConsoleCommand`, then register the command name in `AddConsoleHost`:

```csharp
services.AddConsoleCron(
    commandName: "my:command",
    interval: TimeSpan.FromMinutes(15),
    runOnStartup: true);
```

Crons run with the web app as hosted services. You do not need a separate cron class.
