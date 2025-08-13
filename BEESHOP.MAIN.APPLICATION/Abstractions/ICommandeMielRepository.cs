using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;
using BEESHOP.MAIN.DOMAIN.Common;

namespace BEESHOP.MAIN.APPLICATION.Abstractions;

public interface ICommandeMielRepository : IDbRepository<CommandeMiel>
{
    Task<ListEntity<CommandeMiel>> RecupererCommandesMiel();
    Task<CommandeMiel?> RecupererParCommandeEtMiel(Guid commandeId, Guid mielId);
    Task<IEnumerable<CommandeMielDto>> RecupererParCommande(Guid commandeId);
}
