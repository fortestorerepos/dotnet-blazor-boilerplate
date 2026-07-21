namespace blazor_boilerplate.Providers.Email;

public interface IEmailProvider
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
}
