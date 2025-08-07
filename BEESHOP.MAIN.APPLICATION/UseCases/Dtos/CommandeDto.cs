namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class CommandeDto : IdentifiableDto
{
    public int quantite { get; set; }
    public DateTime dateCommande { get; set; }
    public string userEmail { get; set; }
    public Guid mielId { get; set; }
}
