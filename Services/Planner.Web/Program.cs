using Planner.Web.Host;
using System.Threading;
using System.Threading.Tasks;

namespace Planner.Web
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new PlannerHost();

            await host.RunWebAsync(args, CancellationToken.None);
        }
    }
}
