namespace blazor_boilerplate.Providers.Email;

public sealed record EmailMessage(
    string To,
    string Subject,
    string HtmlBody);
