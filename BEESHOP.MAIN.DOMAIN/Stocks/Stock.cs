using BEESHOP.MAIN.DOMAIN.Common;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.DOMAIN.Stocks;

public class Stock : IdentifiableEntity
{
    [JsonConstructor]
    public Stock(Guid id, Guid mielId, int quantite) : base(id)
    {
        MielId = mielId;
        Quantite = quantite;
    }

    public Stock(Guid mielId, int quantite)
    {
        MielId = mielId;
        Quantite = quantite;
    }

    public Guid MielId { get; set; }

    public int Quantite { get; set; }
}
