using System.ComponentModel.DataAnnotations;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;

public record SectionKeycloak
{
    public const string SectionName = "Keycloak";

    [Required]
    public required string Auth { get; init; }

    [Required]
    public required string Realm { get; init; }

    [Required]
    public required string Secret { get; init; }
}
