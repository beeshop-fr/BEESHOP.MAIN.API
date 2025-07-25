namespace BEESHOP.MAIN.DOMAIN.Common;

public class ListEntity<T>
{
    public ListEntity(List<T> entities)
    {
        Entities = entities;
        Count = entities.Count;
    }

    public int Count { get; private set; }

    public List<T> Entities { get; private set; }
}