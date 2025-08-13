using BEESHOP.MAIN.DOMAIN.Common;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.DOMAIN.Commandes;

public class Commande : IdentifiableEntity
{
    [JsonConstructor]
    public Commande(Guid id, DateTime dateCommande, string userEmail, EStatutCommande statut) : base(id)
    {
        this.dateCommande = dateCommande;
        this.userEmail = userEmail;
        this.statut = statut;
    }
    public Commande(DateTime dateCommande, string userEmail, EStatutCommande statut) : base(Guid.NewGuid())
    {
        this.dateCommande = dateCommande;
        this.userEmail = userEmail;
        this.statut = statut;
    }

    public Commande() : base(Guid.Empty) { }

    public DateTime dateCommande { get; set; }
    public string userEmail { get; set; }
    public EStatutCommande statut { get; set; } = EStatutCommande.EnCours;
}