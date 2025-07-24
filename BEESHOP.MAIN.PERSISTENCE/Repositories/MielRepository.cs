using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.DOMAIN.Miels;
using BEESHOP.MAIN.PERSISTENCE.Common.Repositories;
using Npgsql;
using System.Text.Json;

namespace BEESHOP.MAIN.PERSISTENCE.Repositories;

internal class MielRepository : DbRepository<Miel>, IMielRepository
{
    protected override string TableName => "miels";
    protected override string ColumnName => "miel";
    public MielRepository(NpgsqlDataSource db) : base(db)
    {
    }

    public async Task<List<Miel>> RecupererMiels()
    {
        var result = new List<Miel>();

        using var cmd = _db.CreateCommand($"SELECT {ColumnName} FROM {TableName}");

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var json = reader.GetString(0); // la colonne JSONB est lue comme une string
            var miel = JsonSerializer.Deserialize<Miel>(json);

            if (miel != null)
                result.Add(miel);
        }

        return result;
    }

    public async Task<Miel?> RecupererParId(Guid id)
    {
        // Prépare la commande SQL pour récupérer le miel par son ID
        using var cmd = _db.CreateCommand($"SELECT {ColumnName} FROM {TableName} WHERE id = @id");
        
        cmd.Parameters.AddWithValue("id", id);

        // Exécute la commande et obtient un lecteur de données
        using var reader = await cmd.ExecuteReaderAsync();

        // Vérifie si le lecteur a des lignes
        if (await reader.ReadAsync())
        {
            var json = reader.GetString(0);
            return JsonSerializer.Deserialize<Miel>(json);
        }
        return null; // Si aucun miel n'est trouvé
    }
}