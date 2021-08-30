using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;

namespace Planner.Web.StartupConfiguration
{
    /// <summary>
    /// Configure response compression services and middleware
    /// </summary>
    public static class ResponseCompressionConfigurator
    {
        public static IServiceCollection ConfigureResponseCompressionService(this IServiceCollection services)
        {
            services.AddResponseCompression(o =>
            {
                o.Providers.Add<GzipCompressionProvider>();
                o.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            return services;
        }

        public static void ConfigureResponseCompression(this IApplicationBuilder app) =>
            app.UseResponseCompression();
    }
}