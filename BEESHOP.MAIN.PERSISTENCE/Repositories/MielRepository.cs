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

    public async Task<IReadOnlyList<Miel>> RecupererParIds(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        if (ids is null) throw new ArgumentNullException(nameof(ids));

        // Déduplication + gestion liste vide
        var arr = ids.Distinct().ToArray();
        if (arr.Length == 0)
            return Array.Empty<Miel>();

        await using var conn = await _db.OpenConnectionAsync(ct);

        // ANY(@ids) avec Guid[] → uuid[] en PostgreSQL (inférence Npgsql OK)
        const string sql = @"SELECT * FROM miels WHERE id = ANY(@ids);";

        var cmd = new CommandDefinition(sql, new { ids = arr }, cancellationToken: ct);
        var rows = await conn.QueryAsync<Miel>(cmd);

        return rows.ToList();
    }
}