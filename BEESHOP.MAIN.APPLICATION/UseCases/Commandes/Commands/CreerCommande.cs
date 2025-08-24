using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record CreerCommande(DateTime DateCommande, string UserEmail) : IRequest<CommandeDto>;
