using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DutchTreat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DutchContext>();

            /*services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlServer(_config.GetConnectionString("DutchContextDb"));
            });*/

            services.AddDbContext<DutchContext>();
            services.AddTransient<INullMailService, NullMailService>();
            services.AddTransient<DutchSeeder>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // add IDutchRepository as a service people can use, but use the impl from DutchRepository
            services.AddScoped<IDutchRepository, DutchRepository>();

            
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            
            //the order matters here

            // look for a blank dir URL (in wwwroot) BUT not delivering them
            // app.UseDefaultFiles();

            // looks for the URL provided by UseDefaultFiles
            // listens for the file serving
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapRazorPages();
                cfg.MapControllerRoute("Default", "/{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
            
        }
    }
}
