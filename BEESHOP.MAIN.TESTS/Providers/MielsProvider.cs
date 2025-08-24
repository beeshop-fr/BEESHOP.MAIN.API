using BEESHOP.MAIN.DOMAIN.Miels;

namespace BEESHOP.MAIN.TESTS.Providers;

public static class MielsProvider
{
    public static string fileName = "0904c04e-83e1-4d95-a6f6-162e439794b3.jpg";

    public static Miel GetMielDummy(Guid idMiel)
        => new Miel
        {
            Id = idMiel,
            Nom = "Miel de Lavande",
            TypeMiel = ETypeMiel.EteLiquide,
            Prix = 6.99m,
            Description = "Un miel doux et parfumé.",
            Poids = 500,
            ImagePath = fileName
        };
}
