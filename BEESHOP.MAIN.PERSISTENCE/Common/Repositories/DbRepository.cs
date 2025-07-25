using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.DOMAIN.Common;
using Dapper;
using Npgsql;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Repositories;

public abstract class DbRepository<TBase> : IDbRepository<TBase>
    where TBase : IdentifiableEntity
{
    protected readonly NpgsqlDataSource _db;
    protected abstract string TableName { get; }

    protected DbRepository(NpgsqlDataSource db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public virtual async Task<TBase> Creer(TBase entity)
    {
        using var conn = await _db.OpenConnectionAsync();

        var columns = string.Join(", ", typeof(TBase).GetProperties().Select(p => p.Name.ToLower()));
        var values = string.Join(", ", typeof(TBase).GetProperties().Select(p => "@" + p.Name));

        var sql = $"INSERT INTO {TableName} ({columns}) VALUES ({values});";
        await conn.ExecuteAsync(sql, entity);

        return entity;
    }

    public virtual async Task<TBase?> RecupererParId(Guid id)
    {
        using var conn = await _db.OpenConnectionAsync();

        var sql = $"SELECT * FROM {TableName} WHERE id = @Id;";
        return await conn.QuerySingleOrDefaultAsync<TBase>(sql, new { Id = id });
    }

    public virtual async Task<TBase> Modifier(TBase entity)
    {
        using var conn = await _db.OpenConnectionAsync();

        var properties = typeof(TBase).GetProperties()
            .Where(p => p.Name != "Id")
            .Select(p => $"{p.Name.ToLower()} = @{p.Name}");

        var setClause = string.Join(", ", properties);
        var sql = $"UPDATE {TableName} SET {setClause} WHERE id = @Id;";
        var affected = await conn.ExecuteAsync(sql, entity);

        return affected == 1 ? entity : throw new InvalidOperationException("Update failed.");
    }

    public virtual async Task Supprimer(Guid id)
    {
        using var conn = await _db.OpenConnectionAsync();
        var sql = $"DELETE FROM {TableName} WHERE id = @Id;";
        await conn.ExecuteAsync(sql, new { Id = id });
    }
}
