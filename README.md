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

For local development, copy `Server/appsettings.Development.example.json` to `Server/appsettings.Development.json`, fill in your SMTP values, and keep `Server/appsettings.Development.json` out of git.

```json
{
  "Email": {
    "Host": "smtp.example.com",
    "Port": 587,
    "EnableSsl": true,
    "FromEmail": "no-reply@example.com",
    "FromName": "Blazor Boilerplate",
    "UserName": "smtp-user",
    "Password": "replace-with-secret",
    "TestRecipient": "you@example.com"
  }
}
```

Production should still use real environment variables or the host's secret manager. Environment variables override values from `appsettings.json` and `appsettings.Development.json`.

## Crons

Scheduled jobs run existing console commands. Add the work as an `IConsoleCommand`, then register the command name in `AddConsoleHost`:

```csharp
services.AddConsoleCron(
    commandName: "my:command",
    interval: TimeSpan.FromMinutes(15),
    runOnStartup: true);
```

Crons run with the web app as hosted services. You do not need a separate cron class.
