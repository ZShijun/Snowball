using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
using System.IO;

namespace Snowball.Api
{
    public class StartupDevelopment
    {
        public StartupDevelopment(IConfiguration configuration)
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
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            
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
