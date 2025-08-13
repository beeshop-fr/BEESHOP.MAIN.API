using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record SupprimerCommandeMiel(Guid Id) : IRequest;