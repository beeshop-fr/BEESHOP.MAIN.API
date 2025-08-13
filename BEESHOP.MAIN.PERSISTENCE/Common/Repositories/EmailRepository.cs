using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;
using BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Repositories;

public sealed class EmailRepository : IEmailRepository
{
    private readonly SectionSmtp _sectionSmtp;
    private readonly ILogger<EmailRepository> _logger;

    public EmailRepository(IOptions<SectionSmtp> sectionSmtp, ILogger<EmailRepository> logger)
    {
        _sectionSmtp = sectionSmtp.Value;
        _logger = logger;
    }
    public async Task SendCommandeConfirmee(Commande commande, IEnumerable<CommandeMielDto> commandeMiels, CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("BeeShop", _sectionSmtp.FromAddress));
        message.To.Add(new MailboxAddress("", commande.userEmail));
        message.Subject = $"Confirmation de votre commande #{commande.Id}";

        var bodyBuilder = new BodyBuilder
        {
            TextBody = $"Bonjour {commande.userEmail},\n\n" +
                       $"Votre commande a été confirmée.\n" +
                       $"Détails :\n" +
                       string.Join("\n", commandeMiels.Select(m => $"- {m.mielId} x{m.quantite}")) +
                       $"\n\nMerci pour votre confiance !"
        };

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_sectionSmtp.Host, _sectionSmtp.Port, _sectionSmtp.UseSsl, cancellationToken);
        await client.AuthenticateAsync(_sectionSmtp.Username, _sectionSmtp.Password, cancellationToken);
        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        _logger.LogInformation("Email de confirmation envoyé à {Email}", commande.userEmail);
    }
}