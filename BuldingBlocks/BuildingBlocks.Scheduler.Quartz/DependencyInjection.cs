using BuildingBlocks.Scheduler.Quartz.Models;
using CrystalQuartz.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace BuildingBlocks.Scheduler.Quartz
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQuartzServices<T>(this IServiceCollection services, T collection) where T : JobCollection
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobs = collection.EnumerateJobs();

                foreach (var jobData in jobs)
                {
                    if (!jobData.Execute) continue;

                    var jobKey = new JobKey(jobData.Name);

                    q.AddJob(jobData.Type, jobKey);

                    q.AddTrigger(b =>
                    {
                        b.ForJob(jobKey)
                            .WithIdentity(jobKey + "-trigger")
                            .StartAt(jobData.Trigger.StartAt)
                            .EndAt(jobData.Trigger.EndAt)
                            .WithSimpleSchedule(x =>
                            {
                                if (jobData.Trigger.RepeatCount.HasValue)
                                {
                                    x.WithRepeatCount(jobData.Trigger.RepeatCount.Value);
                                }
                                else
                                {
                                    x.RepeatForever();
                                }

                                x.WithInterval(jobData.Trigger.Interval);
                            });
                    });
                }

            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }

        public static void ConfigureQuartzUi(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var schedulerFactory = scope.ServiceProvider.GetService<ISchedulerFactory>();
            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            app.UseCrystalQuartz(() => scheduler);
        }
    }
}