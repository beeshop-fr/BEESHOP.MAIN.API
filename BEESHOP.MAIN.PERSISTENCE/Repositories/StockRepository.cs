using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.DOMAIN.Common;
using BEESHOP.MAIN.DOMAIN.Stocks;
using BEESHOP.MAIN.PERSISTENCE.Common.Repositories;
using Dapper;
using Npgsql;

namespace BEESHOP.MAIN.PERSISTENCE.Repositories;

public class StockRepository : DbRepository<Stock>, IStockRepository
{
    protected override string TableName => "stocks";
    public StockRepository(NpgsqlDataSource db) : base(db)
    {
        if (db == null)
            throw new ArgumentNullException(nameof(db), "Le NpgsqlDataSource est null !");
    }

    public async Task<ListEntity<Stock>> RecupererStocks()
    {
        using var conn = await _db.OpenConnectionAsync();

        var sql = $"SELECT * FROM {TableName}";
        var result = await conn.QueryAsync<Stock>(sql);
        return new ListEntity<Stock>(result.ToList());
    }
}