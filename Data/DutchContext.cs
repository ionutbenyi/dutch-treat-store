using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

// Dotnet commands for creating the DB:
// 1. dotnet ef database update
// 2. create migration: dotnet ef migrations add InitialDb
// 3. apply migration: dotnet ef database update


// initial: DutchContext : DbContext | For identity, DutchContext : IdentityDbContext

namespace DutchTreat.Data
{
    public class DutchContext : IdentityDbContext<StoreUser>
    {
        private readonly IConfiguration _config;

        public DutchContext(IConfiguration config)
        {
            _config = config;
        }

        // create Db Sets for items you want to directly query against
        public DbSet<Product>  Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>()
                .HasData(new Order()
                {
                    Id = 1,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "124"
                });

        }
    }
}
