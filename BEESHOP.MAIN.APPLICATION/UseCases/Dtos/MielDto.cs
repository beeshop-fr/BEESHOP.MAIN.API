using BEESHOP.MAIN.DOMAIN.Miels;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class MielDto : IdentifiableDto
{
    public required string nom { get; set; }
    public required ETypeMiel type { get; set; }
    public required int prix { get; set; }
    public string description { get; set; }
    public required int poids { get; set; }
}
