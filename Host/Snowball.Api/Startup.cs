using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snowball.Application;
using Snowball.Domain.Bookshelf;
using Snowball.Repositories.Bookshelf;
using System;
using System.IO;
using Snowball.Core;
using Snowball.Core.Utils;
using Snowball.Domain.Wechat.Dtos;
using Snowball.Domain.Wechat;
using Snowball.Repositories.Wechat;
using Snowball.Domain.Stock;
using Snowball.Repositories.Stock;

namespace Snowball.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMySql(options=> {
                options.Default = Configuration.GetValue<string>("ConnectionStrings:Default");
            });

            services.AddHttpClient("danjuanfunds", conf =>
            {
                conf.BaseAddress = new Uri("https://danjuanfunds.com/");
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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Snowball API"
                });
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Snowball.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Snowball API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
