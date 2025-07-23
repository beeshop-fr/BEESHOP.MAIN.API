namespace BEESHOP.MAIN.DOMAIN.Commandes;

public class Commande
{
    public Commande(Guid id, int quantite, DateTime dateCommande, string userEmail, Guid mielId)
    {
        this.id = id;
        this.quantite = quantite;
        this.dateCommande = dateCommande;
        this.userEmail = userEmail;
        this.mielId = mielId;
    }

    public Guid id { get; set; }
    public int quantite { get; set; }
    public DateTime dateCommande { get; set; }
    public string userEmail { get; set; }
    public Guid mielId { get; set; }
}