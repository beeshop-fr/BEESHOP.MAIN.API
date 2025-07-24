using BEESHOP.MAIN.DOMAIN.Common;

namespace BEESHOP.MAIN.DOMAIN.Commandes;

public class Commande : IdentifiableEntity
{
    public Commande(int quantite, DateTime dateCommande, string userEmail, Guid mielId) : base(Guid.NewGuid())
    {
        this.quantite = quantite;
        this.dateCommande = dateCommande;
        this.userEmail = userEmail;
        this.mielId = mielId;
    }

    public int quantite { get; set; }
    public DateTime dateCommande { get; set; }
    public string userEmail { get; set; }
    public Guid mielId { get; set; }
}