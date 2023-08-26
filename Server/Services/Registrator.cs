using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Services
{
    public static class Registrator
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<RandomGenerator>();
            services.AddScoped<BotService>();
            services.AddScoped<OpenAIClient>();

            return services;
        }
    }
}
