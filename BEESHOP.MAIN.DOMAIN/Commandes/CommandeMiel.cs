namespace BEESHOP.MAIN.DOMAIN.Commandes;

public class CommandeMiel
{
    public CommandeMiel(Guid id, Guid commandeId, Guid mielId, int quantite)
    {
        this.id = id;
        this.commandeId = commandeId;
        this.mielId = mielId;
        this.quantite = quantite;
    }

    public Guid id { get; set; }
    public Guid commandeId { get; set; }
    public Guid mielId { get; set; }
    public int quantite { get; set; }
}
