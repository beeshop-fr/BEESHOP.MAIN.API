using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record ValiderCommande : IRequest<bool>
{
    public Guid CommandeId { get; init; }
}