using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Miels;
using Mapster;

namespace BEESHOP.MAIN.APPLICATION;

public static class MapsterConfig
{
    public static void Register()
    {

        TypeAdapterConfig<Miel, MielDto>.NewConfig()
            .Map(d => d.Image, s => s.ImagePath);
    }
}