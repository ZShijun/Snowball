using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Snowball.Core;
using Snowball.Core.Utils;
using Snowball.Domain.Stock;
using Snowball.JobScheduler.Extensions;
using Snowball.Repositories.Stock;
using System;

namespace Snowball.JobScheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();
                        q.AddConfiguredQuartzJobTriggers(hostContext.Configuration);
                    });
                    
                    services.AddMySql(options => {
                        options.Default = hostContext.Configuration.GetValue<string>("ConnectionStrings:Default");
                    });
                    services.AddStockRepository();
                    services.AddStockDomain();
                    services.AddSingleton<TimeProvider, SystemTimeProvider>();
                    services.AddHttpClient("danjuanfunds", conf =>
                    {
                        conf.BaseAddress = new Uri("https://danjuanfunds.com/");
                    });

                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
    }
}
