using Microsoft.Extensions.DependencyInjection;
using Server.DomainFeatures.CounselingRessourceAggregate.Infrastructure;

namespace Server.DomainFeatures.CounselingRessourceAggregate
{
    public static class Registrator
    {
        public static IServiceCollection RegisterCounselingRessourcesModule(this IServiceCollection services)
        {
            services.AddSingleton<CounselingRessourcePersistence>();

            return services;
        }
    }
}
