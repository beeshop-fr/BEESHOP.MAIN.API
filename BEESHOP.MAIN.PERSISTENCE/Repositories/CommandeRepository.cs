using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.DOMAIN.Commandes;
using BEESHOP.MAIN.DOMAIN.Common;
using BEESHOP.MAIN.PERSISTENCE.Common.Repositories;
using Dapper;
using Npgsql;

namespace BEESHOP.MAIN.PERSISTENCE.Repositories;

public class CommandeRepository : DbRepository<Commande>, ICommandeRepository
{
    protected override string TableName => "commande";
    public CommandeRepository(NpgsqlDataSource db) : base(db)
    {
        if (db == null)
            throw new ArgumentNullException(nameof(db), "Le NpgsqlDataSource est null !");
    }

    public async Task<ListEntity<Commande>> RecupererCommandes()
    {
        using var conn = _db.OpenConnectionAsync().Result;

        var sql = $"SELECT * FROM {TableName}";
        var result = await conn.QueryAsync<Commande>(sql);

        return new ListEntity<Commande>(result.ToList());
    }
}