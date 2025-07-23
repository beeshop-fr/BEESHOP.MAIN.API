using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;

public record SectionPostgre
{
    public const string SectionName = "Postgre";

    [Required]
    public required string Host { get; init; }

    [Required]
    public required ushort Port { get; init; }

    [Required]
    public required string Username { get; init; }

    [Required]
    public required string Password { get; init; }

    [Required]
    public required string Database { get; init; }

    [Required]
    public required string Schema { get; init; }

    public string ConnectionString => new NpgsqlConnectionStringBuilder()
    {
        Host = Host,
        Port = Port,
        Username = Username,
        Password = Password,
        Database = Database,
        SearchPath = Schema,
        Enlist = true,
    }.ConnectionString;
}
