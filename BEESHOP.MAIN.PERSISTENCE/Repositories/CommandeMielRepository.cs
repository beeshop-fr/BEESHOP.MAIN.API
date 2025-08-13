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
}