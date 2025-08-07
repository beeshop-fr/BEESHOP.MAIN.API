using BEESHOP.MAIN.DOMAIN.Common;

namespace BEESHOP.MAIN.DOMAIN.Stocks;

public class Stock : IdentifiableEntity
{
    public Stock(Guid id, Guid mielId, int quantite) : base(id)
    {
        MielId = mielId;
        Quantite = quantite;
    }
    public Guid MielId { get; set; }

    public int Quantite { get; set; }
}
