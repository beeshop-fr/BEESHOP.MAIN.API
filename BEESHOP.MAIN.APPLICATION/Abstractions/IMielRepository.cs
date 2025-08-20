using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.DOMAIN.Common;
using BEESHOP.MAIN.DOMAIN.Miels;

namespace BEESHOP.MAIN.APPLICATION.Abstractions;

public interface IMielRepository : IDbRepository<Miel>
{
    Task<ListEntity<Miel>> RecupererMiels();
    Task<IReadOnlyList<Miel>> RecupererParIds(IEnumerable<Guid> ids, CancellationToken ct = default);

}