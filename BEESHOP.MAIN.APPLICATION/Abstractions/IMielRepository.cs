using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.DOMAIN.Miels;

namespace BEESHOP.MAIN.APPLICATION.Abstractions;

public interface IMielRepository : IDbRepository<Miel>
{
    Task<List<Miel>> RecupererMiels();
}