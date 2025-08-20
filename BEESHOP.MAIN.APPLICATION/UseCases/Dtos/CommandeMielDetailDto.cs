namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class CommandeMielDetailDto
{
    public Guid CommandeId { get; set; }
    public Guid MielId { get; set; }
    public string MielNom { get; set; } = default!;
    public decimal PrixUnitaire { get; set; }
    public int Quantite { get; set; }
}
