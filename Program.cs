using DutchTreat.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // if we want to seed the db, don't run the actual hosting

            if (args.Length == 1 && args[0].ToLower() == "/seed")
            {
                RunSeeding(host);
                return;
            }
            
            host.Run();
            
            
        }

        /*private static void RunSeeding(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>(); 
            using (var scope = scopeFactory.CreateScope())
            {
                // get the Dutch Seeder from the context of the scope (in services the dutch seeder is scoped)
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.Seed();
            }
        }*/

        private static void RunSeeding(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                // get the Dutch Seeder from the context of the scope (in services the dutch seeder is scoped)
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.SeedAsync().Wait();
            }
        }

        /*public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
            .UseStartup<Startup>()
            .Build();*/

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(SetupConfiguration)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

        private static void SetupConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json") // dev std file
                .AddEnvironmentVariables(); // override std file (for production)
        }

    }
}
