using BuildingBlocks.Database.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Planner.Application.Common.Interfaces;
using Planner.Persistent.Consts;

namespace Planner.Persistent
{
    /// <summary>
    /// Configure persistent services and middleware
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigurePersistent(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PlannerDbContext>(
                options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString(DefaultConstants.ConnectionStringName))
                        .UseSnakeCaseNamingConvention();
                });

            services.AddScoped<IPlannerDbContext>(provider => provider.GetService<PlannerDbContext>());

            return services;
        }

        public static void UsePersistent(this IApplicationBuilder app)
        {
            app.EnsureContext<PlannerDbContext>();
        }
    }
}