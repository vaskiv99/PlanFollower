using BuildingBlocks.Web.Host;
using Microsoft.AspNetCore.Hosting;

namespace Planner.Web.Host
{
    public class PlannerHost : WebHost<Startup>
    {
        protected override IWebHostBuilder CreateHostBuilder(string[] args)
        {
            return base.CreateHostBuilder(args);
        }
    }
}