using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record CreerCommandeMiel(Guid CommandeId, Guid MielId, int Quantite) : IRequest<CommandeMielDto>;
