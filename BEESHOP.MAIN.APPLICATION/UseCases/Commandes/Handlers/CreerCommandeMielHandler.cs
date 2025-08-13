using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Handlers;

public sealed class CreerCommandeMielHandler : IRequestHandler<CreerCommandeMiel, CommandeMielDto>
{
    private readonly ICommandeMielRepository _commandeMielRepository;
    private readonly ICommandeRepository _commandeRepository;
    private readonly ILogger<CreerCommandeMielHandler> _logger;

    public CreerCommandeMielHandler(ILogger<CreerCommandeMielHandler> logger, ICommandeMielRepository commandeMielRepository, ICommandeRepository commandeRepository)
    {
        _commandeMielRepository = commandeMielRepository;
        _logger = logger;
        _commandeRepository = commandeRepository;
    }
    public async Task<CommandeMielDto> Handle(CreerCommandeMiel request, CancellationToken cancellationToken)
    {
        if (request.CommandeId == Guid.Empty || request.MielId == Guid.Empty)
            throw new ArgumentException("Ids invalides");

        var commande = await _commandeRepository.RecupererParId(request.CommandeId)
                      ?? throw new InvalidOperationException("Commande introuvable");

        if (commande.statut != EStatutCommande.EnCours)
            throw new InvalidOperationException("Commande non modifiable (statut != EnCours)");

        // Upsert : si la ligne existe, on incrémente
        var existante = await _commandeMielRepository.RecupererParCommandeEtMiel(request.CommandeId, request.MielId);

        CommandeMiel ligne;
        if (existante is null)
        {
            ligne = new CommandeMiel(request.CommandeId, request.MielId, request.Quantite);
            ligne = await _commandeMielRepository.Creer(ligne);
        }
        else
        {
            existante.quantite += request.Quantite;
            ligne = await _commandeMielRepository.Modifier(existante);
        }

        _logger.LogInformation("Ajout miel {MielId} x{Qte} à commande {CmdId}",
                               request.MielId, request.Quantite, request.CommandeId);

        return ligne.Adapt<CommandeMielDto>();
    }
}