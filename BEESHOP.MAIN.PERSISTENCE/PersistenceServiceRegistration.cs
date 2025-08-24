using BEESHOP.MAIN.PERSISTENCE.Common.Config.Section;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BEESHOP.MAIN.PERSISTENCE;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var postgreConfig = configuration.GetConfigSection<SectionPostgre>(SectionPostgre.SectionName);
        var keycloakConfig = configuration.GetConfigSection<SectionKeycloak>(SectionKeycloak.SectionName);
        var smtpConfig = configuration.GetConfigSection<SectionSmtp>(SectionSmtp.SectionName);

        services.AddSingleton(smtpConfig);

        return services
            .AddNpgsqlDataSource(postgreConfig!.ConnectionString, o => o.EnableParameterLogging())
            .AddRepositories();
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(PersistenceServiceRegistration).Assembly)
            .AddClasses(classes => classes.Where(c => c.Name.Contains("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}