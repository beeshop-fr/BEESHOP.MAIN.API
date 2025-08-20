using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.DOMAIN.Common;
using BEESHOP.MAIN.DOMAIN.Stocks;

namespace BEESHOP.MAIN.APPLICATION.Abstractions;

public interface IStockRepository : IDbRepository<Stock>
{
    Task<ListEntity<Stock>> RecupererStocks();
}
