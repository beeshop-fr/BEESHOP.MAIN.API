namespace BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;

public record SectionSmtp
{
    public const string SectionName = "Smtp";
    /// <summary>
    ///     SMTP server address.
    /// </summary>
    public required string Host { get; init; }
    /// <summary>
    ///     SMTP server port.
    /// </summary>
    public required int Port { get; init; }
    /// <summary>
    ///    Use STARTTLS for the connection.
    /// </summary>
    public required bool UseStartTls { get; init; }
    /// <summary>
    ///     SMTP username.
    /// </summary>
    public required string Username { get; init; }
    /// <summary>
    ///     SMTP password.
    /// </summary>
    public required string Password { get; init; }
    /// <summary>
    ///    Sender's email address.
    /// </summary>
    public required string FromAddress { get; init; }
    /// <summary>
    ///    Sender's display name.
    /// </summary>
    public string FromDisplayName { get; init; } = "BeeShop";
    /// <summary>
    ///     Use SSL for the connection.
    /// </summary>
    public bool UseSsl { get; init; } = false;
    /// <summary>
    ///   Optional notification address for the beekeeper.
    /// </summary>
    public string? NotificationAddress { get; set; }
}
