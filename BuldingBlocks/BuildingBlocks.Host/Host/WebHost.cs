using BuildingBlocks.Logging;
using BuildingBlocks.Web.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Web.Host
{
    public abstract class WebHost<TStartup> where TStartup : WebStartup
    {
        protected virtual IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new WebHostBuilder().UseStartup<TStartup>()
                .UseKestrel(options => options.AddServerHeader = false)
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }

                    Configuration.ConfigureLoggingSettings(config);
                })
                .ConfigureLogging(Configuration.ConfigureLogging)
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                });

            return builder;
        }

        public async Task RunWebAsync(string[] args, CancellationToken cancellationToken)
        {
            var builder = CreateHostBuilder(args);

            await builder.Build().RunAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}