namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class CommandeDto : IdentifiableDto
{
    public DateTime dateCommande { get; set; }
    public string userEmail { get; set; }
}
