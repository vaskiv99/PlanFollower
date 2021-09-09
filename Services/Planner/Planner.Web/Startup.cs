using BuildingBlocks.Domain;
using BuildingBlocks.Scheduler.Quartz;
using BuildingBlocks.Web.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Planner.Application;
using Planner.Persistent;
using Planner.Web.JobConfiguration;
using Planner.Web.StartupConfiguration;
using DependencyInjection = Planner.Application.DependencyInjection;

namespace Planner.Web
{
    public class Startup : WebStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.ConfigureResponseCompressionService()
                .ConfigurePersistent(Configuration)
                .ConfigureBaseDomainServices()
                .ConfigureApplication()
                .AddQuartzServices(new PlannerJobCollection());
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);

            DependencyInjection.InitializeApplicationMappers();

            app.ConfigureQuartzUi();
            app.UsePersistent();

            app.UseRouting();

            base.ConfigureSwagger(app);
            base.ConfigureCors(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
