using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;

        // tells us info about the env that we're running in
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("ionut@dutchtreat.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName="Andrei",
                    LastName="Ionut",
                    Email= "ionut@dutchtreat.com",
                    UserName= "ionut@dutchtreat.com"
                };

                var result = await _userManager.CreateAsync(user,"P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Cannot create user in seeder");
                }
            }

            if (!_ctx.Products.Any())
            {
                // need to create sample data
                var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                // add them to context
                _ctx.Products.AddRange(products);

                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }
               // _ctx.Orders.Add(order);
                _ctx.SaveChanges();
            }
        }
    }
}
