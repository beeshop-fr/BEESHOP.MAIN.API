using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;
using BEESHOP.MAIN.DOMAIN.Commandes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Handlers;

public class ValiderCommandeHandler : IRequestHandler<ValiderCommande, bool>
{
    private readonly ICommandeRepository _commandeRepository;
    private readonly ICommandeMielRepository _commandeMielRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly ILogger<ValiderCommandeHandler> _logger;
    public ValiderCommandeHandler(ILogger<ValiderCommandeHandler> logger, ICommandeRepository commandeRepository, IEmailRepository emailRepository, ICommandeMielRepository commandeMielRepository)
    {
        _commandeRepository = commandeRepository;
        _commandeMielRepository = commandeMielRepository;
        _logger = logger;
        _emailRepository = emailRepository;
    }
    public async Task<bool> Handle(ValiderCommande request, CancellationToken cancellationToken)
    {
        var commande = await _commandeRepository.RecupererParId(request.CommandeId);
        if (commande == null || commande.statut != EStatutCommande.EnCours)
            throw new InvalidOperationException("Commande invalide.");

        commande.statut = EStatutCommande.Validee;
        await _commandeRepository.Modifier(commande);

        // Récupération de tous les miels liés à la commande
        var commandeMiels = await _commandeMielRepository.RecupererParCommande(request.CommandeId);

        // Envoi de l'email
        await _emailRepository.SendCommandeConfirmee(commande, commandeMiels, cancellationToken);

        return true;
    }
}
