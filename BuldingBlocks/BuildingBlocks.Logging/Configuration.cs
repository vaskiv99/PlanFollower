using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System.Reflection;

namespace BuildingBlocks.Logging
{
    public class Configuration
    {
        public static void ConfigureLoggingSettings(IConfigurationBuilder config)
        {
            var resourcesProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());

            config.AddJsonFile(resourcesProvider, "config.logging.config.json", true, false);
        }

        public static void ConfigureLogging(WebHostBuilderContext context, ILoggingBuilder builder)
        {
            var logger = CreateLogger(context);

            builder.AddSerilog(logger);
        }

        private static Logger CreateLogger(WebHostBuilderContext context)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration);

            var logger = loggerConfiguration.CreateLogger();

            return logger;
        }
    }
}