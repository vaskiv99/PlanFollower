using BuildingBlocks.Database.Seeder;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace BuildingBlocks.Database.Bootstrap
{
    /// <summary>
    /// Service, which ensure EF database context, run migrations and seeder
    /// </summary>
    public static class ContextBootstrapper
    {
        /// <summary>
        /// Ensure EF context
        /// </summary>
        /// <typeparam name="TContext">Database context</typeparam>
        /// <param name="app">Application builder</param>
        public static void EnsureContext<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            var task = app.EnsureContextAsync<TContext>().ConfigureAwait(false);
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Ensure EF context
        /// </summary>
        /// <typeparam name="TContext">Database context</typeparam>
        /// <param name="app">Application builder</param>
        public static async Task EnsureContextAsync<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<TContext>();

            if (context == null)
                return;

            await using (context)
            {
                await context.Database.MigrateAsync().ConfigureAwait(false);

                if (context.Database.IsNpgsql())
                {
                    if (context.Database.GetDbConnection() is NpgsqlConnection connection)
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            await connection.OpenAsync().ConfigureAwait(false);
                        }

                        connection.ReloadTypes();
                    }
                }

                var seeder = scope.ServiceProvider.GetService<ISeeder>();
                if (seeder != null)
                {
                    await seeder.SeedDataAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Ensure EF context
        /// </summary>
        /// <typeparam name="TContext">Database context</typeparam>
        public static void EnsureContext<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            var task = services.EnsureContextAsync<TContext>().ConfigureAwait(false);
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Ensure EF context
        /// </summary>
        /// <typeparam name="TContext">Database context</typeparam>
        public static async Task EnsureContextAsync<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetService<TContext>();

            if (context == null)
                return;

            await using (context)
            {
                await context.Database.MigrateAsync().ConfigureAwait(false);

                if (context.Database.IsNpgsql())
                {
                    if (context.Database.GetDbConnection() is NpgsqlConnection connection)
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            await connection.OpenAsync().ConfigureAwait(false);
                        }

                        connection.ReloadTypes();
                    }
                }

                var seeder = scope.ServiceProvider.GetService<ISeeder>();
                if (seeder != null)
                {
                    await seeder.SeedDataAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
