using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record CheckoutCommande(string UserEmail, IReadOnlyList<LigneCheckout> Lignes) : IRequest<CheckoutResult>;

public record LigneCheckout(Guid MielId, int Quantite);

public record CheckoutResult(Guid CommandeId, string Statut);
