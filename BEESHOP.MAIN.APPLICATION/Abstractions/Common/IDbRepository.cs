using BEESHOP.MAIN.DOMAIN.Common;

namespace BEESHOP.MAIN.APPLICATION.Abstractions.Common;

public interface IDbRepository<TBase>
    where TBase : IdentifiableEntity
{
    Task<TBase> Creer(TBase entity);

    Task<TBase?> RecupererParId(Guid id);

    Task<TBase> Modifier(TBase entity);

    Task Supprimer(Guid id);
}
