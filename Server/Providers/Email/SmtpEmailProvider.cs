using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace blazor_boilerplate.Providers.Email;

public sealed class SmtpEmailProvider(IOptions<EmailOptions> options) : IEmailProvider
{
    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var emailOptions = options.Value;
        ValidateOptions(emailOptions);

        using var mailMessage = new MailMessage
        {
            From = CreateFromAddress(emailOptions),
            Subject = message.Subject,
            Body = message.HtmlBody,
            IsBodyHtml = true
        };

        mailMessage.To.Add(message.To);

        using var smtpClient = new SmtpClient(emailOptions.Host, emailOptions.Port)
        {
            EnableSsl = emailOptions.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(emailOptions.UserName))
        {
            smtpClient.Credentials = new NetworkCredential(emailOptions.UserName, emailOptions.Password);
        }

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }

    private static MailAddress CreateFromAddress(EmailOptions options)
    {
        return string.IsNullOrWhiteSpace(options.FromName)
            ? new MailAddress(options.FromEmail)
            : new MailAddress(options.FromEmail, options.FromName);
    }

    private static void ValidateOptions(EmailOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Host))
        {
            throw new InvalidOperationException("Email:Host must be configured before sending email.");
        }

        if (string.IsNullOrWhiteSpace(options.FromEmail))
        {
            throw new InvalidOperationException("Email:FromEmail must be configured before sending email.");
        }
    }
}
