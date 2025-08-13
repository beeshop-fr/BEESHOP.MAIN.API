using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record SupprimerCommandeHandler(Guid Id) : IRequest;