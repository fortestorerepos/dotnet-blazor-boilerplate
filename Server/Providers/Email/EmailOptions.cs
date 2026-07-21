namespace blazor_boilerplate.Providers.Email;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; } = 587;

    public bool EnableSsl { get; set; } = true;

    public string FromEmail { get; set; } = string.Empty;

    public string? FromName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? TestRecipient { get; set; }
}
