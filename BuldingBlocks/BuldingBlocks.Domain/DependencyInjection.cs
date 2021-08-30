using BuildingBlocks.Domain.Events.Abstractions;
using BuildingBlocks.Domain.Events.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuildingBlocks.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureBaseDomainServices(this IServiceCollection services)
        {
            services.TryAddScoped<IEventBus, EventBus>();

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}