namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class CommandeMielDto : IdentifiableDto
{
    public Guid commandeId { get; set; }
    public Guid mielId { get; set; }
    public int quantite { get; set; }
}
