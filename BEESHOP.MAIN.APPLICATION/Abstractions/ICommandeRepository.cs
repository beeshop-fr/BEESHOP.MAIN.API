using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.DOMAIN.Commandes;
using BEESHOP.MAIN.DOMAIN.Common;

namespace BEESHOP.MAIN.APPLICATION.Abstractions;

public interface ICommandeRepository : IDbRepository<Commande>
{
    Task<ListEntity<Commande>> RecupererCommandes();
}