using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;

public static class ConfigExtension
{
    public static T GetConfigSection<T>(this IConfiguration configuration, string sectionName)
    {
        return configuration.GetRequiredSection(sectionName).Get<T>() ?? throw new Exception($"Config manquante pour {typeof(T).Name}");
    }
}
