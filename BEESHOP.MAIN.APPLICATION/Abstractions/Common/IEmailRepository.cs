using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;

namespace BEESHOP.MAIN.APPLICATION.Abstractions.Common;

public interface IEmailRepository
{
    Task SendCommandeConfirmee(Commande commande, IEnumerable<CommandeMielDto> commandeMielsDto, CancellationToken cancellationToken);
}
