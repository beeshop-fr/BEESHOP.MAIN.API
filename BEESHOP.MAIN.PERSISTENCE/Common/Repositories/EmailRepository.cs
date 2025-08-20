using System.Globalization;
using System.Linq;
using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;
using BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Repositories;

public sealed class EmailRepository : IEmailRepository
{
    private readonly SectionSmtp _sectionSmtp;
    private readonly ILogger<EmailRepository> _logger;

    public EmailRepository(SectionSmtp sectionSmtp, ILogger<EmailRepository> logger)
    {
        _sectionSmtp = sectionSmtp;
        _logger = logger;
        _logger.LogInformation("SMTP: Host={Host}, Port={Port}, StartTls={StartTls}, Ssl={Ssl}, User={User}, From={From}, Bcc={Bcc}");
    }

    public async Task SendCommandeConfirmee(
        Commande commande,
        IEnumerable<CommandeMielDetailDto> commandeMiels,
        CancellationToken cancellationToken)
    {
        var culture = CultureInfo.GetCultureInfo("fr-FR");
        var lignes = commandeMiels?.ToList() ?? new List<CommandeMielDetailDto>();

        // --- Construire le corps du message ---
        var lignesText = string.Join("\n", lignes.Select(m =>
        {
            // Convert.ToDecimal supporte int ou decimal selon ton DTO
            var sousTotal = Convert.ToDecimal(m.PrixUnitaire) * m.Quantite;
            return $"- {m.MielNom} x{m.Quantite} — {sousTotal.ToString("F2", culture)} €";
        }));

        var total = lignes.Sum(m => Convert.ToDecimal(m.PrixUnitaire) * m.Quantite);

        var bodyBuilder = new BodyBuilder
        {
            TextBody =
                $"Bonjour {commande.userEmail},\n\n" +
                $"Votre commande #{commande.Id} a été confirmée.\n\n" +
                $"Détails :\n{lignesText}\n\n" +
                $"Total : {total.ToString("F2", culture)} €\n\n" +
                $"Merci pour votre confiance !"
        };

        // --- Adresses (From / To / Bcc) ---
        var fromRaw = string.IsNullOrWhiteSpace(_sectionSmtp.FromAddress)
            ? _sectionSmtp.Username
            : _sectionSmtp.FromAddress;

        if (string.IsNullOrWhiteSpace(fromRaw))
        {
            _logger.LogError("SMTP FromAddress et Username sont vides. Configure un expéditeur valide.");
            return; // on ne fait pas planter la requête
        }

        if (string.IsNullOrWhiteSpace(commande.userEmail))
        {
            _logger.LogError("Adresse destinataire (commande.userEmail) manquante.");
            return;
        }

        MailboxAddress from;
        MailboxAddress to;

        try
        {
            var display = string.IsNullOrWhiteSpace(_sectionSmtp.FromDisplayName) ? "BeeShop" : _sectionSmtp.FromDisplayName;
            from = new MailboxAddress(display, fromRaw);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Adresse expéditeur invalide: {From}", fromRaw);
            return;
        }

        try
        {
            to = MailboxAddress.Parse(commande.userEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Adresse destinataire invalide: {To}", commande.userEmail);
            return;
        }

        var message = new MimeMessage();
        message.From.Add(from);
        message.To.Add(to);
        // Copie invisible pour l'apiculteur si configurée
        if (!string.IsNullOrWhiteSpace(_sectionSmtp.NotificationAddress))
        {
            try { message.Bcc.Add(MailboxAddress.Parse(_sectionSmtp.NotificationAddress)); }
            catch (Exception ex) { _logger.LogWarning(ex, "NotificationAddress invalide: {Addr}", _sectionSmtp.NotificationAddress); }
        }
        message.Subject = $"Confirmation de votre commande #{commande.Id}";
        message.Body = bodyBuilder.ToMessageBody();

        // --- Connexion SMTP (SSL/STARTTLS) ---
        try
        {
            var secure = _sectionSmtp.UseSsl
                ? SecureSocketOptions.SslOnConnect                 
                : (_sectionSmtp.UseStartTls ? SecureSocketOptions.StartTls
                                            : SecureSocketOptions.Auto);

            using var client = new SmtpClient();

            client.CheckCertificateRevocation = false;

            await client.ConnectAsync(_sectionSmtp.Host, _sectionSmtp.Port, secure, cancellationToken);

            if (!string.IsNullOrWhiteSpace(_sectionSmtp.Username))
                await client.AuthenticateAsync(_sectionSmtp.Username, _sectionSmtp.Password, cancellationToken);

            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Email de confirmation envoyé à {Email}", commande.userEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Échec envoi email pour {Email}", commande.userEmail);
            // ne pas throw : la commande est validée quand même
        }
    }
}
