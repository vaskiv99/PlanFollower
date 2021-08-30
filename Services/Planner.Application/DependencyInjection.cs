using BuildingBlocks.Common.Implementations;
using BuildingBlocks.Common.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Planner.Application.Common.Mappers;
using System.Reflection;

namespace Planner.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddMediatR(Assembly.GetExecutingAssembly())
                .AddTransient<ServiceFactory>(sp => sp.GetRequiredService!);

            return services;
        }

        public static void InitializeApplicationMappers()
            => MapperConfigurationRunner.RunMapperConfigurations(Assembly.GetExecutingAssembly());
    }
}