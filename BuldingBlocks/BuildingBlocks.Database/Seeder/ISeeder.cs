using System.Threading.Tasks;

namespace BuildingBlocks.Database.Seeder
{
    /// <summary>
    /// Service which allow seed data to storage.
    /// </summary>
    public interface ISeeder
    {
        /// <summary>
        /// Seed data.
        /// </summary>
        /// <returns></returns>
        Task SeedDataAsync();
    }
}