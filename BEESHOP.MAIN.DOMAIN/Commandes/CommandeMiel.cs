using BEESHOP.MAIN.DOMAIN.Common;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.DOMAIN.Commandes;

public class CommandeMiel : IdentifiableEntity
{
    [JsonConstructor]
    public CommandeMiel(Guid id, Guid commandeId, Guid mielId, int quantite) : base(id)
    {
        this.commandeId = commandeId;
        this.mielId = mielId;
        this.quantite = quantite;
    }

    public CommandeMiel( Guid commandeId, Guid mielId, int quantite) : base(Guid.NewGuid())
    {
        this.commandeId = commandeId;
        this.mielId = mielId;
        this.quantite = quantite;
    }

    public CommandeMiel() : base(Guid.Empty) { }

    public Guid commandeId { get; set; }
    public Guid mielId { get; set; }
    public int quantite { get; set; }
}
