namespace BEESHOP.MAIN.DOMAIN.Stocks;

public class stock
{
    public stock(Guid id, Guid mielId, int quantite)
    {
        this.id = id;
        this.mielId = mielId;
        this.quantite = quantite;
    }
    public Guid id { get; set; }
    public Guid mielId { get; set; }
    public int quantite { get; set; }
}
