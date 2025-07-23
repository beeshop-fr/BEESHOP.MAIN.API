using BEESHOP.MAIN.APPLICATION.Abstractions.Common;
using BEESHOP.MAIN.DOMAIN.Common;
using Npgsql;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Repositories;

public abstract class DbRepository<TBase> : IDbRepository<TBase>
    where TBase : IdentifiableEntity
{

    protected readonly NpgsqlDataSource _db;

    protected abstract string TableName { get; }
    protected abstract string ColumnName { get; }

    protected DbRepository(NpgsqlDataSource db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }


    public async Task<TBase> Creer(TBase entity)
    {
        var cmd = _db.CreateCommand(
            $"INSERT INTO TABLE {TableName} VALUES (@entity::jsonb"); 

        cmd.Parameters.AddWithValue(ColumnName, Newtonsoft.Json.JsonConvert.SerializeObject(entity));

        var result = await cmd.ExecuteNonQueryAsync();

        return result == 1 ? entity : throw new InvalidOperationException("Failed to create entity in the database.");
    }

    public async Task<TBase> Modifier(TBase entity)
    {
        var cmd = _db.CreateCommand(
            $"UPDATE {TableName} SET @entity::jsonb WHERE {ColumnName} ->> 'id')::uuid = @id");

        cmd.Parameters.AddWithValue(ColumnName, Newtonsoft.Json.JsonConvert.SerializeObject(entity));
        cmd.Parameters.AddWithValue("id", entity.Id);

        var result = await cmd.ExecuteNonQueryAsync();

        return result == 1 ? entity : throw new InvalidOperationException("Failed to update entity in the database.");
    }

    public async Task<TBase?> RecupererParId(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task Supprimer(Guid id)
    {
        throw new NotImplementedException();
    }
}
