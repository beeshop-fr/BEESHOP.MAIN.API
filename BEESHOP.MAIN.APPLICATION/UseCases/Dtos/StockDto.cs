namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class StockDto : IdentifiableDto
{
    public Guid MielId { get; set; }
    public int Quantite { get; set; }
}
