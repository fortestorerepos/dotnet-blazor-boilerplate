using blazor_boilerplate.Data;
using Microsoft.AspNetCore.Identity;

namespace blazor_boilerplate.Providers.Email;

internal sealed class IdentityEmailSender(IEmailProvider emailProvider) : IEmailSender<ApplicationUser>
{
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        return emailProvider.SendEmailAsync(new EmailMessage(
            email,
            "Confirm your email",
            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>."));
    }

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        return emailProvider.SendEmailAsync(new EmailMessage(
            email,
            "Reset your password",
            $"Please reset your password by <a href='{resetLink}'>clicking here</a>."));
    }

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        return emailProvider.SendEmailAsync(new EmailMessage(
            email,
            "Reset your password",
            $"Please reset your password using the following code: {resetCode}"));
    }
}
