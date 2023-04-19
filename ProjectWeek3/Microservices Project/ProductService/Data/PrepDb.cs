using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models;
using ProductService.Data;

namespace ProductService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }
        private static void SeedData(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                Console.WriteLine("==> Seeding Data...");
                context.Products.AddRange(
                new Product()
                {
                    Name = "Laptop",
                    Stock = 80,
                    Description = "Laptop Gaming",
                    Price = 1450
                },
                new Product()
                {
                    Name = "RAM",
                    Stock = 168,
                    Description = "RAM DDR5 64GB",
                    Price = 250
                },
                new Product()
                {
                    Name = "Kursi Gaming",
                    Stock = 25,
                    Description = "Ini kursi bukan sofa",
                    Price = 500
                });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("==> Produk telah tersedia <==");
            }
        }
    }
}