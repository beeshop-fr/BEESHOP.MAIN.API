using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.DOMAIN.Common;
using BEESHOP.MAIN.DOMAIN.Miels;
using BEESHOP.MAIN.PERSISTENCE.Common.Repositories;
using Dapper;
using Npgsql;

namespace BEESHOP.MAIN.PERSISTENCE.Repositories;

public class MielRepository : DbRepository<Miel>, IMielRepository
{
    protected override string TableName => "miels";
    public MielRepository(NpgsqlDataSource db) : base(db) 
    {
        if (db == null)
            throw new ArgumentNullException(nameof(db), "Le NpgsqlDataSource est null !");
    }

    public async Task<ListEntity<Miel>> RecupererMiels()
    {
        using var conn = await _db.OpenConnectionAsync();

        var sql = $"SELECT * FROM {TableName}";
        var result = await conn.QueryAsync<Miel>(sql);
        return new ListEntity<Miel>(result.ToList());
    }
}