using blazor_boilerplate.Providers.Email;
using blazor_boilerplate.Shared.Email;
using Microsoft.Extensions.Options;

namespace blazor_boilerplate.Endpoints;

public static class EmailTestEndpoints
{
    public static IEndpointRouteBuilder MapEmailTestEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/email/test", SendTestEmailAsync)
            .WithName("SendTestEmail");

        return endpoints;
    }

    private static async Task<IResult> SendTestEmailAsync(
        IEmailProvider emailProvider,
        IOptions<EmailOptions> options,
        CancellationToken cancellationToken)
    {
        var recipient = options.Value.TestRecipient;
        if (string.IsNullOrWhiteSpace(recipient))
        {
            return Results.BadRequest(new EmailTestResult(
                false,
                "Configure Email:TestRecipient before sending a test email."));
        }

        try
        {
            await emailProvider.SendEmailAsync(new EmailMessage(
                recipient,
                "Test email from Blazor Boilerplate",
                "<p>Your Blazor Boilerplate email service sent this test message successfully.</p>"),
                cancellationToken);
        }
        catch (Exception exception)
        {
            return Results.Json(
                new EmailTestResult(false, exception.Message),
                statusCode: StatusCodes.Status500InternalServerError);
        }

        return Results.Ok(new EmailTestResult(true, $"Test email sent to {recipient}."));
    }
}
