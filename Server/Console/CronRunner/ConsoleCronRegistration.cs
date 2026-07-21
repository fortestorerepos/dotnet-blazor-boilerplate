namespace blazor_boilerplate.Console.Crons;

public sealed record ConsoleCronRegistration(
    string CommandName,
    string[] Arguments,
    TimeSpan Interval,
    bool RunOnStartup);
