using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalePurchaseAccountant.BLL;

namespace SalePurchaseAccountant.Api
{
    public class Startup
    {
        private IConfiguration Config { get; }

        public Startup(IConfiguration Configuration)
        {
            Config = Configuration;
            if(Config["ConnectionString:IntegratedSecurity"] == "true")
            {
                Connection.Initialize(Config["ConnectionString:Server"], Config["ConnectionString:Database"]);
            }
            else
            {
                Connection.Initialize(Config["ConnectionString:Server"], Config["ConnectionString:Database"], Config["ConnectionString:UserId"], Config["ConnectionString:Password"]);
            }
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to Configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:2001", "http://localhost:2020","http://192.168.0.111:2001", "http://localhost:4200", "http://192.168.0.111:2002")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddMvc(option => 
            option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
        }

        // This method gets called by the runtime. Use this method to Configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowOrigin");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
