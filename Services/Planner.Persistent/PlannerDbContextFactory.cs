using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Planner.Persistent.Consts;
using System;
using System.IO;

namespace Planner.Persistent
{
    public class PlannerDbContextFactory : IDesignTimeDbContextFactory<PlannerDbContext>
    {
        public PlannerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlannerDbContext>();

            optionsBuilder.UseNpgsql(GetConnectionString(args))
                .UseSnakeCaseNamingConvention();

            return new PlannerDbContext(optionsBuilder.Options);
        }

        private static string GetConnectionString(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
                .AddJsonFile($"appsettings.{envName}.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return configuration.GetConnectionString(DefaultConstants.ConnectionStringName)
                   ?? throw new ArgumentException("Connection string must be in configuration");
        }
    }
}