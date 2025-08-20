using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.DOMAIN.Common;

public class IdentifiableEntity
{
    public Guid Id { get; init; }

    [JsonConstructor]
    protected IdentifiableEntity(Guid id)
    {
        Id = id;
    }

    protected IdentifiableEntity()
    {
        Id = Guid.NewGuid();
    }
}
