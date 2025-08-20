using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public sealed class CreerCommandeHandler : IRequestHandler<CreerCommande, CommandeDto>
{
    private readonly ICommandeRepository _commandeRepository;
    private readonly ILogger<CreerCommandeHandler> _logger;

    public CreerCommandeHandler(ILogger<CreerCommandeHandler> logger, ICommandeRepository commandeRepository)
    {
        _commandeRepository = commandeRepository;
        _logger = logger;
    }
    public async Task<CommandeDto> Handle(CreerCommande request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création d'une commande pour l'utilisateur : {UserEmail}", request.UserEmail);

        if (string.IsNullOrWhiteSpace(request.UserEmail))
            throw new ArgumentException("UserEmail requis");

        //var existant = await _commandeRepository
        //    .RecupererPanierEnCoursParUser(request.UserEmail, ct);
        //if (existant is not null)
        //{
        //    _logger.LogInformation("Panier déjà existant pour {UserEmail} : {Id}", request.UserEmail, existant.Id);
        //    return existant.Adapt<CommandeDto>();
        //}

        var date = request.DateCommande == default ? DateTime.UtcNow : request.DateCommande;
        Commande commande = new(date, request.UserEmail, EStatutCommande.EnCours);

        commande = await _commandeRepository.Creer(commande);

        return commande.Adapt<CommandeDto>();
    }
}