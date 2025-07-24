using BEESHOP.MAIN.DOMAIN.Miels;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class MielDto : IdentifiableDto
{
    public required string Nom { get; set; }
    public required ETypeMiel TypeMiel { get; set; }
    public required int Prix { get; set; }
    public string Description { get; set; }
    public required int Poids { get; set; }
}
