using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Commandes;
using BEESHOP.MAIN.DOMAIN.Common;
using BEESHOP.MAIN.PERSISTENCE.Common.Repositories;
using Dapper;
using Npgsql;

namespace BEESHOP.MAIN.PERSISTENCE.Repositories;

public class CommandeMielRepository : DbRepository<CommandeMiel>, ICommandeMielRepository
{
    protected override string TableName => "commande_miel";
    public CommandeMielRepository(NpgsqlDataSource db) : base(db)
    {
        if (db == null)
            throw new ArgumentNullException(nameof(db), "Le NpgsqlDataSource est null !");
    }

    public async Task<ListEntity<CommandeMiel>> RecupererCommandesMiel()
    {
        using var conn = await _db.OpenConnectionAsync();
        var sql = $"SELECT * FROM {TableName}";
        var result = await conn.QueryAsync<CommandeMiel>(sql);
        return new ListEntity<CommandeMiel>(result.ToList());
    }

    public async Task<CommandeMiel?> RecupererParCommandeEtMiel(Guid commandeId, Guid mielId)
    {
        await using var conn = await _db.OpenConnectionAsync();
        var sql = $@" SELECT Id, CommandeId, MielId, Quantite FROM {TableName} WHERE CommandeId = @CommandeId AND MielId = @MielId LIMIT 1;";
        return await conn.QueryFirstOrDefaultAsync<CommandeMiel>(sql, new { commandeId, mielId });
    }

    public async Task<IEnumerable<CommandeMielDto>> RecupererParCommande(Guid commandeId)
    {
        await using var conn = await _db.OpenConnectionAsync();
        var sql = $@"  
       SELECT cm.CommandeId, cm.MielId, cm.Quantite  
       FROM {TableName} cm  
       WHERE cm.CommandeId = @CommandeId;";
        return await conn.QueryAsync<CommandeMielDto>(sql, new { commandeId });
    }

    public async Task UpsertQuantite(Guid commandeId, Guid mielId, int quantite, CancellationToken ct = default)
    {
        if (quantite < 0) throw new ArgumentOutOfRangeException(nameof(quantite));

        await using var conn = await _db.OpenConnectionAsync(ct);

        var sql = $"INSERT INTO {TableName} (id, commandeid, mielid, quantite) VALUES (gen_random_uuid(), @commandeId, @mielId, @quantite) ON CONFLICT (commandeid, mielid) DO UPDATE SET quantite = EXCLUDED.quantite;";  

        var cmd = new CommandDefinition(sql, new { commandeId, mielId, quantite }, cancellationToken: ct);
        await conn.ExecuteAsync(cmd);
    }

    public async Task<IReadOnlyList<CommandeMielDto>> RecupererParCommande(Guid commandeId, CancellationToken ct = default)
    {
        await using var conn = await _db.OpenConnectionAsync(ct);

        var sql = $"SELECT id, commandeid AS commandeId, mielid AS mielId, quantite FROM {TableName} WHERE commandeid = @commandeId ORDER BY id;";

        var cmd = new CommandDefinition(sql, new { commandeId }, cancellationToken: ct);
        var rows = await conn.QueryAsync<CommandeMielDto>(cmd);
        return rows.ToList();
    }
}
