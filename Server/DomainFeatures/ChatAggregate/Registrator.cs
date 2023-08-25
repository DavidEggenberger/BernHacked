using Microsoft.Extensions.DependencyInjection;
using Server.DomainFeatures.ChatAggregate.Infrastructure;

namespace Server.DomainFeatures.ChatAggregate
{
    public static class Registrator
    {
        public static IServiceCollection RegisterChatModule(this IServiceCollection services)
        {
            services.AddSingleton<ChatPersistence>();

            return services;
        }
    }
}
