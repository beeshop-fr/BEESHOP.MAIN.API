using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;
using BEESHOP.MAIN.DOMAIN.Commandes;
using MediatR;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Handlers;

public class CheckoutCommandeHandler : IRequestHandler<CheckoutCommande, CheckoutResult>
{
    private readonly ICommandeRepository _cmdRepo;
    private readonly ICommandeMielRepository _ligneRepo;
    private readonly IMielRepository _mielRepo;
    private readonly IEmailRepository _emailRepo;

    public CheckoutCommandeHandler(
        ICommandeRepository cmdRepo,
        ICommandeMielRepository ligneRepo,
        IMielRepository mielRepo,
        IEmailRepository emailRepo)
    {
        _cmdRepo = cmdRepo;
        _ligneRepo = ligneRepo;
        _mielRepo = mielRepo;
        _emailRepo = emailRepo;
    }

    public async Task<CheckoutResult> Handle(CheckoutCommande req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.UserEmail))
            throw new ArgumentException("Email requis");
        if (req.Lignes is null || req.Lignes.Count == 0)
            throw new ArgumentException("Le panier est vide");
        if (req.Lignes.Any(l => l.Quantite <= 0))
            throw new ArgumentException("Quantité invalide");

        // 1) Créer la commande EnCours
        var commande = await _cmdRepo.Creer(new Commande(DateTime.UtcNow, req.UserEmail, EStatutCommande.EnCours));

        // 2) Upsert des lignes (remplacement exact de la quantité)
        foreach (var l in req.Lignes)
            await _ligneRepo.UpsertQuantite(commande.Id, l.MielId, l.Quantite, ct);

        // 3) Charger les détails (pour email lisible)
        var lignes = await _ligneRepo.RecupererParCommande(commande.Id);
        var mielIds = lignes.Select(x => x.mielId).Distinct().ToList();
        var miels = await _mielRepo.RecupererParIds(mielIds); // ajoute cette méthode simple
        var map = miels.ToDictionary(m => m.Id);                  // m.Nom, m.Prix etc.

        // 4) Valider la commande
        commande.statut = EStatutCommande.Validee;
        await _cmdRepo.Modifier(commande);

        // 5) Email après validation (simple)
        await _emailRepo.SendCommandeConfirmee(
            commande,
            lignes.Select(l => new CommandeMielDetailDto
            {
                CommandeId = l.commandeId,
                MielId = l.mielId,
                MielNom = map[l.mielId].Nom,
                PrixUnitaire = map[l.mielId].Prix,
                Quantite = l.quantite
            }),
            ct
        );

        return new CheckoutResult(commande.Id, "Validee");
    }
}