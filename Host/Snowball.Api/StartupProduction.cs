using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snowball.Application;
using Snowball.Core;
using Snowball.Core.Utils;
using Snowball.Domain.Bookshelf;
using Snowball.Domain.Stock;
using Snowball.Domain.Wechat;
using Snowball.Domain.Wechat.Dtos;
using Snowball.Repositories.Bookshelf;
using Snowball.Repositories.Stock;
using Snowball.Repositories.Wechat;
using System;

namespace Snowball.Api
{
    public class StartupProduction
    {
        public StartupProduction(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMySql(options => {
                options.Default = Configuration.GetValue<string>("ConnectionStrings:Default");
            });

            // 配置Hsts
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 443;
            });

            services.AddSingleton<TimeProvider, SystemTimeProvider>();
            services.Configure<WechatOption>(Configuration.GetSection("Wechat"));
            services.AddBookshelfRepository();
            services.AddBookshelfDomain();
            services.AddWechatRepository();
            services.AddWechatDomain();
            services.AddStockRepository();
            services.AddStockDomain();

            services.AddApplication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
