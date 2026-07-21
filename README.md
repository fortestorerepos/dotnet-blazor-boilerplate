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

## Email configuration

Email settings are read from environment variables. Copy `.env.example` to `.env` for local runs, fill in your SMTP values, and keep `.env` out of git.

```bash
Email__Host=smtp.example.com
Email__Port=587
Email__EnableSsl=true
Email__FromEmail=no-reply@example.com
Email__FromName=Blazor Boilerplate
Email__UserName=smtp-user
Email__Password=replace-with-secret
Email__TestRecipient=you@example.com
```

ASP.NET Core reads real environment variables automatically in production. The Makefile exports `.env` values for local `make dev`, `make start`, and `make console` runs.

## Crons

Scheduled jobs run existing console commands. Add the work as an `IConsoleCommand`, then register the command name in `AddConsoleHost`:

```csharp
services.AddConsoleCron(
    commandName: "my:command",
    interval: TimeSpan.FromMinutes(15),
    runOnStartup: true);
```

Crons run with the web app as hosted services. You do not need a separate cron class.
